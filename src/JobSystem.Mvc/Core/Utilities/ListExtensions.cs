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
using System.Collections.Generic;
using System.Web.Mvc;

namespace JobSystem.Mvc.Core.Utilities
{
    public static class ListExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
            return collection;
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection)
        {
            return new SelectList(collection, "Id", "Name");
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection, object selectedValue)
        {
            return new SelectList(collection, "Id", "Name", selectedValue != null ? selectedValue.ToString() : "");
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection, string selectedValue)
        {
            return new SelectList(collection, "Id", "Name", selectedValue);
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection, string dataValueField, string dataTextField)
        {
            return new SelectList(collection, dataValueField, dataTextField);
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection, string dataValueField, string dataTextField, string selectedValue)
        {
            return new SelectList(collection, dataValueField, dataTextField, selectedValue);
        }
    }
}