// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Management.EventHub.Models
{
    public partial class EHNamespaceIdContainer
    {
        internal static EHNamespaceIdContainer DeserializeEHNamespaceIdContainer(JsonElement element)
        {
            string id = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    id = property.Value.GetString();
                    continue;
                }
            }
            return new EHNamespaceIdContainer(id);
        }
    }
}