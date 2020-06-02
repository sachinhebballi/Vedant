using System;
using System.Linq;

namespace SGMH.Healthcare.Vedant.API.Validators
{
    public class CustomValidator
    {
        /// <summary>
        /// Regex that matches numbers with spaces
        /// </summary>
        public const string NumbersWithoutSpaces = @"^[0-9]*$";

        /// <summary>
        /// Regex that matches numbers with spaces
        /// </summary>
        public const string Username = @"^[a-zA-Z0-9 ._@]*$";

        /// <summary>
        /// Validates the format of date
        /// </summary>
        /// <param name="dateValue">Input date value</param>
        /// <returns>Returns a boolean indicating whether date is valid or not</returns>
        public static bool CheckDateFormat(string dateValue)
        {
            DateTime dDate;

            if (DateTime.TryParseExact(dateValue, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out dDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsBoolean(string x)
        {
            return x.ToLowerInvariant() == "y" || x.ToLowerInvariant() == "n";
        }

        public static bool IsNotAZero(string streetNumber)
        {
            var result = true;
            if (!streetNumber.ToCharArray().Any(Char.IsLetter))
            {
                int.TryParse(streetNumber, out int intStreetNumber);
                result = intStreetNumber != 0;
            }

            return result;
        }

        //public static bool ValidateOfferTypes(string x)
        //{
        //    var result = Enum.GetValues(typeof(OfferTypes))
        //            .OfType<OfferTypes>()
        //            .ToList()
        //            .Where(o => o != OfferTypes.Unknown)
        //            .Select(y => Helpers.EnumHelper.GetDescription(y))
        //            .Any(e => e.ToLowerInvariant().Equals(x.ToLowerInvariant()));

        //    return result;
        //}

        /// <summary>
        /// Validates the age as per business rules
        /// </summary>
        /// <param name="dateValue">Birth date</param>
        /// <returns>Returns a boolean indicating whether the age is valid or not</returns>
        public static bool ValidateAge(string dateValue)
        {
            DateTime birthDate;

            DateTime.TryParse(dateValue,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out birthDate);

            DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"),
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime today);

            var age = today.ToUniversalTime().Year - birthDate.ToUniversalTime().Year;

            if (birthDate.ToUniversalTime().Date > today.AddYears(-age)) age--;

            return age < 110;
        }
    }
}
