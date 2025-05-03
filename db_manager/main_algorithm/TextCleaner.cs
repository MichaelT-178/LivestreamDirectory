using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

/**
 * Methods to clean and transform text
 *
 * Methods
 * CleanText | Cleans text by calling all the other functions.
 * NormalizeToAscii | Converts non-ascii chars into standard ascii chars
 * RemoveNonAlphanumeric | Replace any sequence of non-alphanumeric characters with a single space.
 * CollapseSpaces | Replace multiple spaces with a single dash.
 * LowercaseText | Convert the text to lowercase.
 *
 * @author Michael Totaro
 */
public static class TextCleaner
{

    /**
     * Cleans text by calling all the other functions.
     * Text will be all lowercase, ascii, separated by dashes.
     *
     * Ex. 
     * original -> Pronounced 'Lĕh-'nérd 'Skin-'nérd
     * cleared -> pronounced-leh-nerd-skin-nerd
     *
     * @param text Uncleaned text
     * @return cleaned text
     */
    public static string CleanText(string text)
    {
        string ascii = NormalizeToAscii(text);
        string alphanumeric = RemoveNonAlphanumeric(ascii);
        string dashed = CollapseSpaces(alphanumeric);
        string cleaned = LowercaseText(dashed);

        return cleaned;
    }


    /**
     * Converts non-ascii chars into standard ascii chars
     *
     * Ex. 
     * original -> Pronounced 'Lĕh-'nérd 'Skin-'nérd
     * normalized -> Pronounced 'Leh-'nerd 'Skin-'nerd
     * 
     * @param text Text with non-ascii characters
     * @return the text replaced with ascii characters
     */
    public static string NormalizeToAscii(string text)
    {
        string normalized = text.Normalize(NormalizationForm.FormD);

        StringBuilder asciiBuilder = new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
            
            if (category != UnicodeCategory.NonSpacingMark)
            {
                asciiBuilder.Append(c);
            }
        }

        return asciiBuilder.ToString().Normalize(NormalizationForm.FormC);
    }


    /**
     * Replace any sequence of non-alphanumeric characters with a single space.
     * 
     * Ex. 
     * original -> Pronounced 'Leh-'nerd 'Skin-'nerd
     * normalized -> Pronounced Leh nerd Skin nerd
     *
     * @param text Text containing chars that aren't numbers or spaces.
     * @return Text that contains only numbers and spaces.
     */
    public static string RemoveNonAlphanumeric(string text)
    {
        return Regex.Replace(text, @"[^a-zA-Z0-9]+", " ").Trim();
    }


    /**
     * Replace multiple spaces with a single dash.
     *
     * Ex. 
     * original -> Pronounced Leh nerd Skin nerd
     * collapsed -> Pronounced-Leh-nerd-Skin-nerd
     *
     * @param text Text with spaces
     * @return Text with spaces replaced by dash.
     */
    public static string CollapseSpaces(string text)
    {
        return Regex.Replace(text, @"\s+", "-").Trim();
    }

    /**
     * Convert text to lowercase.
     *
     * Ex. 
     * original -> Pronounced-Leh-nerd-Skin-nerd
     * lowercase -> pronounced-leh-nerd-skin-nerd
     *
     * @param text The text to be turned to lowercase
     * @return The lowercase version of the text.
     */
    public static string LowercaseText(string text)
    {
        return text.ToLowerInvariant();
    }
}
