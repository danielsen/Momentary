using System;
using System.IO;
using System.Net.Mail;
using Momentary.Data;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.Data
{
    [TestFixture]
    public class RandomDataTests
    {
        [Test]
        public void should_join()
        {
            var result = new string[] {"a", "b", "c"}.Join("-");
            Assert.AreEqual("a-b-c", result);
        }

        [Test]
        public void should_get_short()
        {
            var value = RandomData.RandomInt16();
            Assert.IsTrue(value >= short.MinValue);
            Assert.IsTrue(value <= short.MaxValue);
        }

        [Test]
        public void should_get_int()
        {
            var value = RandomData.RandomInt32();
            Assert.IsInstanceOf<int>(value);
        }

        [Test]
        public void should_get_int_in_range()
        {
            var value = RandomData.RandomInt32(5, 10);
            Assert.IsInstanceOf<int>(value);
            Assert.IsTrue(value >= 5);
            Assert.IsTrue(value <= 10);
        }

        [Test]
        public void should_exclude_int()
        {
            var value = RandomData.RandomInt32(2, 4, 3);
            Assert.AreNotEqual(3, value);
        }

        [Test]
        public void should_get_random_month([Values(1, 0)] int exclude)
        {
            var value = RandomData.RandomMonth(exclude);
            Assert.IsInstanceOf<int>(value);
            Assert.IsTrue(value >= 1);
            Assert.IsTrue(value <= 12);
            Assert.AreNotEqual(exclude, value);
        }

        [Test]
        public void should_get_random_month_with_excludes()
        {
            var value = RandomData.RandomMonth(null, 3, 4);
            Assert.IsInstanceOf<int>(value);
            Assert.IsTrue(value >= 1);
            Assert.IsTrue(value <= 12);
            Assert.AreNotEqual(3, value);
            Assert.AreNotEqual(4, value);
        }

        [Test]
        public void should_get_random_future_year()
        {
            var exclude = DateTime.UtcNow.Year + 2;
            var value = RandomData.RandomFutureYear(exclude);
            Assert.IsInstanceOf<int>(value);
            Assert.IsTrue(value >= DateTime.UtcNow.Year + 1);
            Assert.IsTrue(value <= DateTime.UtcNow.Year + 11);
            Assert.AreNotEqual(exclude, value);
        }

        [Test]
        public void should_get_random_future_year_with_excludes()
        {
            var exclude = DateTime.UtcNow.Year + 2;
            var value = RandomData.RandomFutureYear(null, exclude);
            Assert.IsInstanceOf<int>(value);
            Assert.IsTrue(value >= DateTime.UtcNow.Year + 1);
            Assert.IsTrue(value <= DateTime.UtcNow.Year + 11);
            Assert.AreNotEqual(exclude, value);
        }

        [Test]
        public void should_get_random_decimal()
        {
            var value = RandomData.RandomDecimal(2);
            Assert.IsInstanceOf<decimal>(value);
        }

        [Test]
        public void should_get_double()
        {
            var value = RandomData.RandomDouble(2);
            Assert.IsInstanceOf<double>(value);
        }

        [Test]
        public void should_get_random_char()
        {
            var value = RandomData.RandomChar();
            Assert.IsInstanceOf<char>(value);
        }

        [Test]
        public void should_get_random_byte()
        {
            var value = RandomData.RandomByte();
            Assert.IsInstanceOf<byte>(value);
        }

        [Test]
        public void should_get_random_bytes()
        {
            var value = RandomData.RandomBytes(2);
            Assert.IsInstanceOf<byte[]>(value);
        }

        [Test]
        public void should_get_random_binary_stream()
        {
            using (var value = RandomData.RandomBinaryStream(10))
            {
                Assert.IsInstanceOf<MemoryStream>(value);
                Assert.AreEqual(10, value.Length);
            }
        }

        [Test]
        public void should_get_random_utf8_stream()
        {
            using (var value = RandomData.RandomUTF8Stream(10))
            {
                Assert.IsInstanceOf<MemoryStream>(value);
                Assert.AreEqual(10, value.Length);
            }
        }

        [Test]
        public void should_get_random_email()
        {
            var value = RandomData.RandomEmail();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_email_as_mail_address()
        {
            var value = RandomData.RandomMailAddress();
            Assert.IsInstanceOf<MailAddress>(value);
        }

        [Test]
        public void should_get_random_phone()
        {
            var value = RandomData.RandomPhone();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_normalized_phone_number()
        {
            var value = RandomData.RandomNormalizedPhone();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_credit_card_number()
        {
            var value = RandomData.RandomCreditCardNumber();
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(16, value.Length);
        }

        [Test]
        public void should_get_random_cvc()
        {
            var value = RandomData.RandomCreditCardCVC();
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(3, value.Length);
        }

        [Test]
        public void should_get_random_domain()
        {
            var value = RandomData.RandomDomain();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_upper_alpha_string()
        {
            var value = RandomData.RandomUpperAlphaString(20);
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(20, value.Length);
        }

        [Test]
        public void should_get_random_alpha_string()
        {
            var value = RandomData.RandomAlphaString(20);
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(20, value.Length);
        }

        [Test]
        public void should_get_random_words()
        {
            var value = RandomData.RandomWords(10);
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_sentence()
        {
            var value = RandomData.RandomSentence(10);
            Assert.IsInstanceOf<string>(value);
            var words = value.Split(' ');
        }

        [Test]
        public void should_get_random_numeric_string()
        {
            var value = RandomData.RandomNumericString(30);
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(30, value.Length);
        }

        [Test]
        public void should_get_random_street_address()
        {
            var value = RandomData.RandomStreetAddress1();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_street_address_apartment()
        {
            var value = RandomData.RandomStreetAddress2();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_us_zip()
        {
            var value = RandomData.RandomUSZip();
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(6, value.Length);
        }

        [Test]
        public void should_get_random_us_state()
        {
            var value = RandomData.RandomStateCode("AK");
            Assert.IsInstanceOf<string>(value);
            Assert.AreNotEqual("AK", value);
        }

        [Test]
        public void should_get_random_country_code()
        {
            var value = RandomData.RandomCountryCode("US");
            Assert.IsInstanceOf<string>(value);
            Assert.AreNotEqual("US", value);
        }

        [Test]
        public void should_get_random_ip_address()
        {
            var value = RandomData.RandomIpAddress();
            Assert.IsInstanceOf<string>(value);
            Assert.IsTrue(value.StartsWith("10"));
        }

        [Test]
        public void should_get_random_hex_color()
        {
            var value = RandomData.RandomHexColor();
            Assert.IsInstanceOf<string>(value);
        }

        [Test]
        public void should_get_random_url()
        {
            var value = RandomData.RandomUrl();
            Assert.IsInstanceOf<string>(value);
        }
    }
}