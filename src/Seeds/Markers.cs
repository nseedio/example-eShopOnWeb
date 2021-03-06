﻿using System;

namespace Seeds
{
    // NSEED-vNEXT:
    // This class will be expanded with addtional markers and moved to NSeed.
    internal static class Markers
    {
        /// <summary>
        /// Converts a GUID to its short string representation marker.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A typical short string representation of a <see cref="Guid"/> looks like this one "K-ROb0hYA0KbtCGZpBVv2g".<br/>
        /// These kind of markers are perfect to inject into largers body of text like e.g. descriptions.
        /// </para>
        /// <para>
        /// Short string representations always have the length of 22 characters and can contain only the following characters:<br/>
        /// <ul>
        /// <li>uppercase characters "A" to "Z"</li>
        /// <li>the lowercase characters "a" to "z"</li>
        /// <li>the numerals "0" to "9"</li>
        /// <li>the symbols "_" and "-"</li>
        /// </ul>
        /// </para>
        /// </remarks>
        /// <param name="guid">The GUID that has to be converted to its short string representation marker.</param>
        /// <returns>
        /// The short string representation marker of the <paramref name="guid"/>.
        /// </returns>
        public static string ToStringMarker(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()) // Returns something similar to "K/ROb0hYA0KbtCGZpBVv2g==".
                                                              // The value will always have length of 24 characters and two trailing "==".
                                                              // Also, it can contain the "/" and "+" signs.
                            .Substring(0, 22)
                            .Replace('/', '_')
                            .Replace('+', '-');
        }
    }
}
