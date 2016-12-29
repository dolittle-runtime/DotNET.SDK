﻿#region License
//
// Copyright (c) 2008-2015, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using Bifrost.Validation.MetaData;
using FluentValidation.Validators;

namespace Bifrost.FluentValidation.MetaData
{
    /// <summary>
    /// Represents the generater that can generate a <see cref="Email"/> rule from
    /// a <see cref="IEmailValidator"/>
    /// </summary>
    public class EmailGenerator : ICanGenerateRule
    {
#pragma warning disable 1591 // Xml Comments
        public Type[] From { get { return new[] { typeof(IEmailValidator), typeof(EmailValidator) }; } }

        public Rule GeneratorFrom(string propertyName, IPropertyValidator propertyValidator)
        {
            var emailRule = new Email
            {
                Message = propertyValidator.GetErrorMessageFor(propertyName)
            };
            return emailRule;
        }
#pragma warning restore 1591 // Xml Comments
    }
}
