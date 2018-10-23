// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreODataSample.Web.Models
{
    public class Person
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IDictionary<string, object> DynamicProperties { get; set; }

        public Level MyLevel { get; set; }
    }
}
