using System.Text;
public static class Encryption
{
    public static string EncryptDecrypt(string inputText, int key)
    {
        StringBuilder outputStringBuild = new StringBuilder (inputText.Length);
        for (int i=0; i < inputText.Length;i++)
        {
            char ch = (char)(inputText[i] ^ key);
            outputStringBuild.Append(ch);
        }
        return outputStringBuild.ToString();
    }
}
