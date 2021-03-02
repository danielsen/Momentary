using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Momentary.Threading;

namespace Momentary.Data
{
    public static class RandomData
    {
        private static readonly Random Random = new Random();

        private static readonly string[] LoremIpsumWords =
        {
            "Aenean", "porta", "massa", "id", "tincidunt", "euismod", "Nulla", "lacinia", "bibendum", "turpis",
            "sollicitudin", "iaculis", "velit", "tempus", "vel", "Nulla", "facilisi", "Aenean", "lobortis", "enim",
            "eget", "metus", "aliquam", "fermentum", "Nam", "ultrices", "leo", "a", "sollicitudin", "dignissim",
            "ipsum", "augue", "tincidunt", "ligula", "quis", "finibus", "lorem", "neque", "ac", "erat", "Pellentesque",
            "molestie", "justo", "in", "est", "scelerisque", "interdum", "Nullam", "nec", "rutrum", "metus", "Nulla",
            "malesuada", "felis", "quam", "quis", "fermentum", "ipsum", "rhoncus", "vel", "Nam", "libero", "ante",
            "posuere", "non", "ipsum", "a", "feugiat", "varius", "lorem", "Nulla", "in", "laoreet", "nunc", "a",
            "ullamcorper", "dolor", "Orci", "varius", "natoque", "penatibus", "et", "magnis", "dis", "parturient",
            "montes", "nascetur", "ridiculus", "mus", "Suspendisse", "eu", "elit", "dictum", "augue", "aliquet",
            "rutrum", "Nunc", "nec", "tortor", "vel", "nisi", "faucibus", "ullamcorper", "In", "nec", "lorem", "a",
            "eros", "porta", "semper", "Fusce", "nec", "sapien", "vel", "ligula", "faucibus", "condimentum", "Sed",
            "eu", "eleifend", "eros", "Praesent", "eget", "hendrerit", "diam", "Integer", "ac", "metus", "porttitor",
            "laoreet", "tellus", "sollicitudin", "mollis", "tellus", "Praesent", "ultricies", "molestie", "sem", "sit",
            "amet", "consectetur", "Donec", "non", "dignissim", "dolor", "Vivamus", "sodales", "sollicitudin", "lectus",
            "at", "mattis", "Nullam", "felis", "neque", "iaculis", "in", "ante", "vel", "fermentum", "hendrerit",
            "lectus", "Nam", "cursus", "ornare", "libero", "sit", "amet", "fermentum", "justo", "hendrerit", "id",
            "Nam", "vitae", "sem", "non", "tellus", "mattis", "facilisis", "sit", "amet", "et", "libero", "Nam",
            "auctor", "rutrum", "lacus", "ut", "porttitor", "Quisque", "bibendum", "diam", "vel", "urna", "rutrum",
            "ac", "viverra", "dui", "iaculis", "Integer", "sodales", "ex", "eu", "vulputate", "viverra", "libero",
            "nibh", "rhoncus", "nulla", "eget", "tempor", "dolor", "urna", "sed", "massa", "Praesent", "diam", "lacus",
            "porttitor", "viverra", "magna", "sit", "amet", "tincidunt", "finibus", "libero", "Donec", "tincidunt",
            "leo", "et", "purus", "tristique", "eget", "tincidunt", "elit", "placerat", "Suspendisse", "potenti"
        };
        
        private static readonly ThreadSafeRandom WordRandom = new ThreadSafeRandom();

        public static string LoremIpsum(this int length)
        {
            var result = "";
            while (true)
            {
                var word = LoremIpsumWords.OrderBy(x => WordRandom.Next())
                    .Select(x => result.Any() ? $" {x}" : x)
                    .FirstOrDefault(x => x.Length <= length - result.Length);
                if (word != null) result += word;
                else break;
            }
            return result;
        }

        public static string LoremIpsumWord(this int length)
        {
            return LoremIpsumWords.OrderBy(x => WordRandom.Next())
                .FirstOrDefault(x => x.Length >= length);
        }
        
        public static string Join<T>(this IEnumerable<T> items, string delimiter = "")
        {
            if (items == null) return string.Empty;
            var itemsArray = items as T[] ?? items.ToArray();
            return !itemsArray.Any()
                ? string.Empty
                : itemsArray.Select(x => x.ToString()).Aggregate((a, i) => $"{a}{delimiter}{i}");
        }

        public static short RandomInt16()
        {
            return (short) ExcludeResults(() => Random
                .Next(short.MinValue, short.MaxValue), 0);
        }

        public static int RandomInt32()
        {
            return ExcludeResults(() => Random.Next(), 0);
        }

        public static int RandomMonth(params int[] exclude)
        {
            return RandomInt32(1, 12, exclude);
        }

        public static int RandomMonth(int? exclude1, params int?[] exclude)
        {
            return RandomMonth(exclude.Prepend(exclude1).Where(x => x.HasValue)
                .Select(x => x.Value).ToArray());
        }

        public static int RandomFutureYear(params int[] exclude)
        {
            return RandomInt32(DateTime.UtcNow.Year + 1, DateTime.UtcNow.Year + 11, exclude);
        }

        public static int RandomFutureYear(int? exclude1, params int?[] exclude)
        {
            return RandomFutureYear(exclude.Prepend(exclude1).Where(x => x.HasValue)
                .Select(x => x.Value).ToArray());
        }

        public static int RandomInt32(int minValue, int maxValue, params int[] exclude)
        {
            return ExcludeResults(() => Random.Next(minValue, maxValue),
                exclude.Any() ? exclude : new[] {0});
        }

        public static decimal RandomDecimal(int significantFigures)
        {
            return ExcludeResults(() => ((decimal) Random.NextDouble())
                .RoundTo(significantFigures + 2) * 100, 0);
        }

        public static double RandomDouble(int significantFigures)
        {
            return ExcludeResults(() => Random.NextDouble()
                .RoundTo(significantFigures + 2) * 100, 0);
        }

        private static T ExcludeResults<T>(Func<T> generator, params T[] exclude)
        {
            while (true)
            {
                var randomValue = generator();
                if (!exclude.Contains(randomValue)) return randomValue;
            }
        }

        public static char RandomChar()
        {
            return (char) RandomInt32(97, 122);
        }

        public static byte RandomByte()
        {
            return (byte) RandomInt32(1, 255);
        }

        public static byte[] RandomBytes(int length)
        {
            var bytes = new byte[length];
            Random.NextBytes(bytes);
            return bytes;
        }

        public static MemoryStream RandomBinaryStream(int length)
        {
            return new MemoryStream(RandomBytes(length));
        }

        public static MemoryStream RandomUTF8Stream(int length)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(RandomAlphaString(length)));
        }

        public static string RandomEmail()
        {
            return $"{RandomWords(10).Replace(" ", ".").ToLower()}@{RandomDomain()}";
        }

        public static MailAddress RandomMailAddress()
        {
            return new MailAddress(RandomEmail());
        }

        public static string RandomPhone()
        {
            return $"({RandomNumericString(3)}) {RandomNumericString(3)}-{RandomNumericString(4)}";
        }

        public static string RandomNormalizedPhone()
        {
            var areaCode = RandomNumericString(3);
            while (areaCode.StartsWith("1"))
            {
                areaCode = RandomNumericString(3);
            }

            return $"{areaCode}{RandomNumericString(3)}{RandomNumericString(4)}";
        }

        public static string RandomCreditCardNumber()
        {
            return RandomNumericString(16);
        }

        public static string RandomCreditCardCVC()
        {
            return RandomNumericString(3);
        }

        public static string RandomDomain()
        {
            return $"{RandomWords(10).Replace(" ", "-").ToLower()}.com";
        }

        public static string RandomUpperAlphaString(int length = 20,
            int start = 97, int end = 122)
        {
            return RandomAlphaString(length, start, end);
        }

        public static string RandomAlphaString(int length = 20,
            int start = 97, int end = 122)
        {
            return 1.To(length).Select(x =>
                (char) Random.Next(start, end)).Join();
        }

        public static string RandomWords(int length = 20)
        {
            return length.LoremIpsum();
        }

        public static string RandomSentence(int length = 20)
        {
            return length.LoremIpsum() + ".";
        }

        public static string RandomWord(int length = 1)
        {
            return length.LoremIpsumWord();
        }

        public static string RandomNumericString(int length = 20)
        {
            var alphaString = RandomAlphaString(length, 48, 57);
            if (alphaString.StartsWith("0"))
            {
                alphaString = alphaString.Remove(0, 1) + "0";
            }

            return alphaString;
        }

        public static string RandomStreetAddress1()
        {
            return $"{RandomNumericString(4)} {RandomWord(10)} Street";
        }

        public static string RandomStreetAddress2()
        {
            return $"Suite {RandomNumericString(3)}";
        }

        public static string RandomUSZip()
        {
            return RandomNumericString(6);
        }

        private static readonly string[] StateCodes =
        {
            "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA",
            "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA",
            "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY",
            "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX",
            "UT", "VT", "VA", "WA", "WV", "WI", "WY"
        };

        public static string RandomStateCode(params string[] exclude)
        {
            return StateCodes.OrderBy(x => Random.Next()).FirstOrDefault(x => !exclude.Contains(x));
        }

        private static readonly string[] CountryCodes =
        {
            "AF", "AL", "DZ", "AD", "AO", "AI", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS",
            "BH", "BD", "BB", "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BA", "BW", "BV", "BR", "IO",
            "VG", "BN", "BG", "BF", "BI", "KH", "CM", "CA", "CV", "KY", "CF", "TD", "CL", "CN", "CX",
            "CC", "CO", "KM", "CG", "CD", "CK", "CR", "CI", "HR", "CU", "CY", "CZ", "DK", "DJ", "DM",
            "DO", "TL", "EC", "EG", "SV", "GQ", "ER", "EE", "ET", "FK", "FO", "FJ", "FI", "FR", "GF",
            "PF", "TF", "GA", "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP", "GT", "GN", "GW",
            "GY", "HT", "HM", "VA", "HN", "HK", "HU", "IS", "IN", "ID", "IQ", "IE", "IR", "IL", "IT",
            "JM", "JP", "JO", "KZ", "KE", "KI", "KP", "KR", "KW", "KG", "LA", "LV", "LB", "LS", "LR",
            "LY", "LI", "LT", "LU", "MO", "MK", "MG", "MW", "MY", "MV", "ML", "MT", "MH", "MQ", "MR",
            "MU", "YT", "MX", "MD", "MC", "MN", "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "AN",
            "NC", "NZ", "NI", "NE", "NG", "NU", "NF", "NO", "OM", "PK", "PW", "PA", "PG", "PY", "PE",
            "PH", "PN", "PL", "PT", "QA", "RE", "RO", "RU", "RW", "SH", "KN", "LC", "PM", "VC", "WS",
            "SM", "ST", "SA", "SN", "RS", "SC", "SL", "SG", "SK", "SI", "SB", "SO", "ZA", "GS", "ES",
            "LK", "SD", "SR", "SJ", "SZ", "SE", "CH", "SY", "TW", "TJ", "TZ", "TH", "TG", "TK", "TO",
            "TT", "TN", "TR", "TM", "TC", "TV", "UG", "UA", "AE", "GB", "US", "UY", "UZ", "VU", "VE",
            "VN", "WF", "EH", "YE", "ZM", "ZW",
        };

        public static string RandomCountryCode(params string[] exclude)
        {
            return CountryCodes.OrderBy(x => Random.Next()).FirstOrDefault(x => !exclude.Contains(x));
        }

        // Ensure test IPs are internal only and never a production IP
        public static string RandomIpAddress()
        {
            return $"10.{Random.Next(1, 255)}." +
                   $"{Random.Next(1, 255)}.{Random.Next(1, 255)}";
        }

        public static string RandomHexColor()
        {
            return Random.Next().ToHexColorString();
        }

        public static string RandomUrl()
        {
            return $"http://www.{RandomDomain()}";
        }

        private static decimal RoundTo(this decimal num, int places)
        {
            return Math.Round(num, places);
        }

        private static double RoundTo(this double num, int places)
        {
            return Math.Round(num, places);
        }

        private static IEnumerable<int> To(this int start, int end)
        {
            return end >= start ? Enumerable.Range(start, end - start + 1) : Enumerable.Empty<int>();
        }

        private static string ToHexColorString(this int value)
        {
            return $"#{value:X6}";
        }
    }
}