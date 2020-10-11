namespace POC.Storage
{
    /// <summary>
    /// Extension methods for Code field related abstractions.
    /// </summary>
    public static class CodeExtensions
    {
        /// <summary>
        /// Compares two code configurations.
        /// </summary>
        /// <param name="cc1">The first code configuration.</param>
        /// <param name="cc2">The second code configuration.</param>
        /// <returns></returns>
        public static bool SameAs(this CodeConfiguration? cc1, CodeConfiguration? cc2)
        {
            if (cc1 == null && cc2 == null)
            {
                return true;
            }
            if (cc1 == null || cc2 == null)
            {
                return false;
            }
            if (cc1.Code == null && cc2.Code == null)
            {
                return true;
            }
            if (cc1.Code == null || cc2.Code == null)
            {
                return false;
            }
            if (cc1.Type != cc2.Type)
            {
                return false;
            }
            if (cc1.Code.Count != cc2.Code.Count)
            {
                return false;
            }
            for (int i = 0; i < cc1.Code.Count; i++)
            {
                var code1 = cc1.Code[i];
                var code2 = cc2.Code[i];
                if (!code1.SameAs(code2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares two code options.
        /// </summary>
        /// <param name="co1">The first code option.</param>
        /// <param name="co2">The second code option.</param>
        /// <returns></returns>
        public static bool SameAs(this CodeOption? co1, CodeOption? co2)
        {
            if (co1 == null && co2 == null)
            {
                return true;
            }
            if (co1 == null || co2 == null)
            {
                return false;
            }
            if (co1.Subcode == null && co2.Subcode == null)
            {
                return true;
            }
            if (co1.Subcode == null || co2.Subcode == null)
            {
                return true;
            }
            if (co1.Value != co2.Value ||
                !co1.Subcode.SameAs(co2.Subcode))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares two subcode options.
        /// </summary>
        /// <param name="sco1">The first subcode option.</param>
        /// <param name="sco2">The second subcode option.</param>
        /// <returns></returns>
        public static bool SameAs(this SubcodeOption? sco1, SubcodeOption? sco2)
        {
            if (sco1 == null && sco2 == null)
            {
                return true;
            }
            if (sco1 == null || sco2 == null)
            {
                return false;
            }
            if (sco1.Values.Count != sco1.Values.Count)
            {
                return false;
            }
            if (sco1.Type != sco2.Type ||
                sco1.IsRequired != sco2.IsRequired)
            {
                return false;
            }
            for (int i = 0; i < sco1.Values.Count; i++)
            {
                var value1 = sco1.Values[i];
                var value2 = sco2.Values[i];
                if (value1 != value2)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares two code field values.
        /// </summary>
        /// <param name="cfv1">The first code field value.</param>
        /// <param name="cfv2">The second code field value.</param>
        /// <returns></returns>
        public static bool SameAs(this CodeFieldValue? cfv1, CodeFieldValue? cfv2)
        {
            if (cfv1 == null && cfv2 == null)
            {
                return true;
            }
            if (cfv1 == null || cfv2 == null)
            {
                return false;
            }
            if (cfv1.Code == null && cfv2.Code == null)
            {
                return true;
            }
            if (cfv1.Code == null || cfv2.Code == null)
            {
                return false;
            }
            if (cfv1.Code.Count != cfv2.Code.Count)
            {
                return false;
            }
            for (int i = 0; i < cfv1.Code.Count; i++)
            {
                var code1 = cfv1.Code[i];
                var code2 = cfv2.Code[i];
                if (!code1.SameAs(code2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares two code values.
        /// </summary>
        /// <param name="cv1">The first code value.</param>
        /// <param name="cv2">The second code value.</param>
        /// <returns></returns>
        public static bool SameAs(this CodeValue? cv1, CodeValue? cv2)
        {
            if (cv1 == null && cv2 == null)
            {
                return true;
            }
            if (cv1 == null || cv2 == null)
            {
                return false;
            }
            if (cv1.Subcode == null && cv2.Subcode == null)
            {
                return true;
            }
            if (cv1.Subcode == null || cv2.Subcode == null)
            {
                return false;
            }
            if (!cv1.Subcode.SameAs(cv2.Subcode))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares two subcode values.
        /// </summary>
        /// <param name="scv1">The first subcode value.</param>
        /// <param name="scv2">The second subcode value.</param>
        /// <returns></returns>
        public static bool SameAs(this SubcodeValue? scv1, SubcodeValue? scv2)
        {
            if (scv1 == null && scv2 == null)
            {
                return true;
            }
            if (scv1 == null || scv2 == null)
            {
                return false;
            }
            if (scv1.Value == null && scv2.Value == null)
            {
                return true;
            }
            if (scv1.Value == null || scv2.Value == null)
            {
                return false;
            }
            if (scv1.Value.Count != scv2.Value.Count)
            {
                return false;
            }
            for (int i = 0; i < scv1.Value.Count; i++)
            {
                if (scv1.Value[i] != scv2.Value[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
