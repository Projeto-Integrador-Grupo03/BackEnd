namespace ecommerce_iniciativatena.Security
{
    public class Settings
    {
        private static string secret = "ac4187a6da0ffd7ce0cd959122d3ac6b32356ea05fdac0a81e1f9793e4820225";

        public static string Secret { get => secret; set => secret = value; }
    }
}
