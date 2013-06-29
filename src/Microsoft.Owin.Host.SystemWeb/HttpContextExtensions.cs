﻿// <copyright file="HttpContextExtensions.cs" company="Microsoft Open Technologies, Inc.">
// Copyright 2013 Microsoft Open Technologies, Inc. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

#if NET45

using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;

namespace System.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Claims;

    /// <summary>Provides extension methods for <see cref="HttpContext"/>.</summary>
    public static partial class HttpContextExtensions
    {
        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="authenticationTypes"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task Authenticate(this HttpContext context, string[] authenticationTypes, Action<IIdentity,
            IDictionary<string, string>, IDictionary<string, object>, object> callback, object state)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            return owinContext.Authentication.Authenticate(authenticationTypes, callback, state);
        }

        public static IEnumerable<AuthenticationDescription> GetAuthenticationTypes(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            return owinContext.Authentication.GetAuthenticationTypes();
        }

        public static IEnumerable<AuthenticationDescription> GetAuthenticationTypes(this HttpContext context,
            Func<AuthenticationDescription, bool> predicate)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            return owinContext.Authentication.GetAuthenticationTypes(predicate);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static void SignIn(this HttpContext context, ClaimsIdentity user)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.SignIn(new AuthenticationExtra(), user);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="extra"></param>
        public static void SignIn(this HttpContext context, ClaimsIdentity user, AuthenticationExtra extra)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.SignIn(extra, user);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="authenticationTypes"></param>
        public static void SignOut(this HttpContext context, params string[] authenticationTypes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.SignOut(authenticationTypes);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="authenticationTypes"></param>
        public static void Challenge(this HttpContext context, params string[] authenticationTypes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.Challenge(new AuthenticationExtra(), authenticationTypes);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="extra"></param>
        /// <param name="authenticationTypes"></param>
        public static void Challenge(this HttpContext context, AuthenticationExtra extra, string authenticationTypes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.Challenge(extra, authenticationTypes);
        }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="extra"></param>
        /// <param name="authenticationTypes"></param>
        public static void Challenge(this HttpContext context, AuthenticationExtra extra, params string[] authenticationTypes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (extra == null)
            {
                throw new ArgumentNullException("extra");
            }

            IOwinContext owinContext = GetOwinContext(context);
            owinContext.Authentication.Challenge(extra, authenticationTypes);
        }

        private static IDictionary<string, object> GetOwinEnvironment(this HttpContext context)
        {
            return (IDictionary<string, object>)context.Items[HttpContextItemKeys.OwinEnvironmentKey];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IOwinContext GetOwinContext(this HttpContext context)
        {
            IDictionary<string, object> environment = GetOwinEnvironment(context);

            if (environment == null)
            {
                throw new InvalidOperationException(
                    Microsoft.Owin.Host.SystemWeb.Resources.HttpContext_OwinEnvironmentNotFound);
            }

            return new OwinContext(environment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IOwinContext GetOwinContext(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return request.RequestContext.HttpContext.GetOwinContext();
        }
    }
}

#endif
