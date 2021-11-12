﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NorthwoodLib.Pools;

using UnityEngine;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Atlas.ModManager.Configs.Converters
{
    /// <summary>
    /// Converts a Vector2, Vector3 or Vector4 to Yaml configs and vice versa.
    /// </summary>
    public sealed class VectorsConverter : IYamlTypeConverter
    {
        /// <inheritdoc/>
        public bool Accepts(Type type) => type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4);

        /// <inheritdoc/>
        public object ReadYaml(IParser parser, Type type)
        {
            if (!parser.TryConsume<MappingStart>(out _))
                throw new InvalidDataException($"Cannot deserialize object of type {type.FullName}.");

            var coordinates = ListPool<object>.Shared.Rent(4);
            var i = 0;

            while (!parser.TryConsume<MappingEnd>(out _))
            {
                if (i++ % 2 == 0)
                {
                    parser.MoveNext();
                    continue;
                }

                if (!parser.TryConsume<Scalar>(out var scalar) || !float.TryParse(scalar.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var coordinate))
                {
                    ListPool<object>.Shared.Return(coordinates);
                    throw new InvalidDataException($"Invalid float value.");
                }

                coordinates.Add(coordinate);
            }

            object vector = Activator.CreateInstance(type, coordinates.ToArray());

            ListPool<object>.Shared.Return(coordinates);

            return vector;
        }

        /// <inheritdoc/>
        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var coordinates = new Dictionary<string, float>(4);

            if (value is Vector2 vector2)
            {
                coordinates["x"] = vector2.x;
                coordinates["y"] = vector2.y;
            }
            else if (value is Vector3 vector3)
            {
                coordinates["x"] = vector3.x;
                coordinates["y"] = vector3.y;
                coordinates["z"] = vector3.z;
            }
            else if (value is Vector4 vector4)
            {
                coordinates["x"] = vector4.x;
                coordinates["y"] = vector4.y;
                coordinates["z"] = vector4.z;
                coordinates["w"] = vector4.w;
            }

            emitter.Emit(new MappingStart());

            foreach (var coordinate in coordinates)
            {
                emitter.Emit(new Scalar(coordinate.Key));
                emitter.Emit(new Scalar(coordinate.Value.ToString()));
            }

            emitter.Emit(new MappingEnd());
        }
    }
}
