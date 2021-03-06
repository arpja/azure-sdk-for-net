// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.SystemEvents
{
    /// <summary> Schema of the chat thread member. </summary>
    public partial class ACSChatThreadMemberProperties
    {
        /// <summary> Initializes a new instance of ACSChatThreadMemberProperties. </summary>
        internal ACSChatThreadMemberProperties()
        {
        }

        /// <summary> Initializes a new instance of ACSChatThreadMemberProperties. </summary>
        /// <param name="displayName"> The name of the user. </param>
        /// <param name="memberId"> The MRI of the user. </param>
        internal ACSChatThreadMemberProperties(string displayName, string memberId)
        {
            DisplayName = displayName;
            MemberId = memberId;
        }

        /// <summary> The name of the user. </summary>
        public string DisplayName { get; }
        /// <summary> The MRI of the user. </summary>
        public string MemberId { get; }
    }
}
