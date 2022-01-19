namespace VaultKeeper {
    public class TextSerializer {
        public static string LoadText(byte[] bytes) {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        
        public static byte[] GetTextBytes(string text) {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }
    }
}