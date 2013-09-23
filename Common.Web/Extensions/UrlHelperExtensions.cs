namespace System.Web.Mvc {
    using System.Text;

    public static class UrlHelperExtensions {
        public static string ToFriendly(this UrlHelper helper, string value) {
            string prepared = (value ?? "").Trim().ToLower();
            StringBuilder url = new StringBuilder();

            foreach (char ch in prepared) {
                switch (ch) {
                    case ' ':
                        url.Append('-');
                        break;
                    case '&':
                        url.Append("and");
                        break;
                    case '\'':
                        break;
                    case '.':
                        url.Append("-dot-");
                        break;
                    default:
                        if ((ch >= '0' && ch <= '9') ||
                            (ch >= 'a' && ch <= 'z')) {
                            url.Append(ch);
                        } else {
                            url.Append('-');
                        }
                        break;
                }
            }

            var result = url.ToString();
            while (result.IndexOf("--") != -1) {
                result = result.Replace("--", "-");
            }

            return result.Trim('-');
        }
    }
}
