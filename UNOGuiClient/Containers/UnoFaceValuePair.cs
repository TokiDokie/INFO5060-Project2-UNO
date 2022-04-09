﻿/* File Name:       GameWindow.xaml.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System.Text;
using System.Windows.Media;

namespace UNOGuiClient.Containers
{
    public class UnoFaceValuePair
    {
        public string CardValue { get; set; }

        // Gets the card value without underscores and uppercases each word
        public string GetFormattedCardValue
        {
            get
            {
                if (!(CardValue is null))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    string[] cardValueTokens = CardValue.Split('_');

                    for (int i = 0; i < cardValueTokens.Length; i++)
                    {
                        string token = cardValueTokens[i];

                        if (string.IsNullOrEmpty(token))
                        {
                            continue;
                        }

                        stringBuilder.Append(string.Concat(token[0].ToString().ToUpper(), token.Substring(1)));

                        if (i < cardValueTokens.Length - 1)
                        {
                            stringBuilder.Append(' ');
                        }
                    }

                    return stringBuilder.ToString();
                }

                return "";
            }
        }

        public ImageSource CardFace { get; set; }

        public UnoFaceValuePair(string cardValue, ImageSource cardFace)
        {
            CardValue = cardValue;
            CardFace = cardFace;
        }
    }
}
