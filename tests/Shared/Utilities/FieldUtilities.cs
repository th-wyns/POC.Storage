using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    static class FieldUtilities
    {

        internal static Field GetRandomField(FieldType type, string? name = null)
        {
            name = name ?? $"Field_{RndUtilities.NextInt32()}";
            return new Field(name: name)
            {
                Description = $"Description of {name}",
                Type = type,
                IsBuiltIn = RndUtilities.NextBool(),
                IsIncludeInTextSearch = RndUtilities.NextBool(),
                IsRelational = RndUtilities.NextBool(),
                IsRequiredOnCodeSets = RndUtilities.NextBool(),
                IsComputed = RndUtilities.NextBool(),
                CodeConfiguration = type == FieldType.Code ? GetTestCodeConfiguration() : null
            };
        }

        internal static async Task<Field> CreateRandomFieldAsync(this IMetadataProvider metadataProvider, FieldType type, string? id = null)
        {
            var field = GetRandomField(type, id);
            await metadataProvider.Field.CreateAsync(field, CancellationToken.None);
            return field;
        }

        internal static async Task<Field> CreateRandomFieldAsync(this Storage storage, FieldType type, string? id = null)
        {
            return await storage.Metadata.CreateRandomFieldAsync(type, id);
        }

        internal static async Task CreateFieldAsync(this IMetadataProvider metadataProvider, Field field)
        {
            await metadataProvider.Field.CreateAsync(field, CancellationToken.None);
        }

        internal static async Task CreateFieldAsync(this Storage storage, Field field)
        {
            await storage.Metadata.CreateFieldAsync(field);
        }

        internal static object GetRandomValue(FieldType type)
        {
            switch (type)
            {
                case FieldType.LongText:
                    return RndUtilities.NextString(RndUtilities.Next(0, 1024 * 1024)); // max 1 MB
                case FieldType.FixedLengthText:
                    return RndUtilities.NextString(RndUtilities.Next(0, 451));
                case FieldType.Date:
                    return new DateTime(RndUtilities.Next(1000, 9000), RndUtilities.Next(1, 13), RndUtilities.Next(1, 28));
                case FieldType.Number:
                    // TODO: determine range
                    //return (decimal)rnd.NextDouble() * (decimal)Math.Pow(10, rnd.Next(29));
                    //return rnd.NextDecimal();
                    return RndUtilities.NextInt32();
                case FieldType.Code:
                    return GetTestCodeValue();
                default:
                    throw new NotImplementedException($"Missing random generator for type: {type.ToString()}");
            }
        }

        internal static CodeConfiguration GetTestCodeConfiguration()
        {
            var codeConfiguration = new CodeConfiguration()
            {
                Type = CodeType.SingleSelect,
                Code =
                {
                    {
                        new CodeOption {
                            Value = "SingleOne",
                            Subcode = new SubcodeOption()
                            {
                                IsRequired = true,
                                Type = CodeType.SingleSelect,
                                Values = { "SingleOne_SingleOne", "SingleOne_SingleTwo", "SingleOne_SingleThree" }
                            }
                        }
                    },
                    new CodeOption
                    {
                        Value = "SingleTwo",
                        Subcode = new SubcodeOption()
                        {
                            IsRequired = false,
                            Type = CodeType.MultiSelect,
                            Values = { "SingleTwo_MultiOne", "SingleTwo_MultiTwo", "SingleTwo_MultiThree" }
                        }
                    }
                }
            };
            return codeConfiguration;
        }

        internal static CodeFieldValue GetTestCodeValue()
        {
            return new CodeFieldValue
            {
                Code = {
                    new CodeValue {
                        Value = "SingleOne",
                        Subcode =
                            new SubcodeValue {
                                Value = { "SingleOne_MultiOne", "SingleOne_MultiTwo" }
                            }
                    }
                }
            };
        }
    }
}
