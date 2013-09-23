namespace System {
    public static class GuidExtensions {
        public static string Encode(this Guid target) {
            // Convert the supplied GUID to a base 64 string.
            string base64 = Convert.ToBase64String(target.ToByteArray());

            // Remove all encoding characters from the string.
            string encoded = base64.Replace("/", "_").Replace("+", "-");

            // Return the encoded string.
            return encoded.Substring(0, 22);
        }
    }
}