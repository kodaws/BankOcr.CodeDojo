﻿namespace BankOcr.Parser;

public static class AccountNumberFormatter
{
    public static string Format(AccountValidationResult validatedAccount)
    {
        return validatedAccount.Match(
            van => van.RecognizedDigits.FormatAccountDigits(),
            invLen => invLen.RecognizedDigits.FormatAccountDigits() + " LEN",
            invChk => invChk.RecognizedDigits.FormatAccountDigits() + " ERR",
            unrecChars => unrecChars.RecognizedDigits.FormatAccountDigits() + " ILL",
            ambNum => " TODO");
    }

    private static string FormatAccountDigits(this IEnumerable<RecognitionResult> digits)
    {
        return string.Join("",
            digits.Select(g =>
                g.Match(
                    rg => rg.DigitPrototype.Digit.ToString(),
                    _ => "?")));
    }
}