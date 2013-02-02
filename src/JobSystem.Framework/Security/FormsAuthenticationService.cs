// <copyright company="Gael Limited">
// Copyright (c) 2010 All Right Reserved
// </copyright>
// <author></author>
// <email></email>
// <date>2010</date>
// <summary>
//    Complying with all copyright laws is the responsibility of the 
//    user. Without limiting rights under copyrights, neither the 
//    whole nor any part of this document may be reproduced, stored 
//    in or introduced into a retrieval system, or transmitted in any 
//    form or by any means (electronic, mechanical, photocopying, 
//    recording, or otherwise), or for any purpose without express 
//    written permission of Gael Limited.
// </summary>

using System;
using System.Web.Security;

namespace JobSystem.Framework.Security
{
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Sets the auth cookie.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
    }
}