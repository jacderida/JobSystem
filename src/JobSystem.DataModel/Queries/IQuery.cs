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

using System.Linq;

namespace JobSystem.DataModel.Queries
{
    /// <summary>
    /// Implementation of Specification pattern.
    /// http://en.wikipedia.org/wiki/Specification_pattern 
    /// </summary>
    /// <typeparam name="T">The type of object to filter</typeparam>
    public interface IQuery<T>
    {
        /// <summary>
        /// Returns a filtered list of tyoe T
        /// </summary>
        /// <param name="values">The list to filter</param>
        /// <returns>The filtered list of values</returns>
        IQueryable<T> Matches(IQueryable<T> values);

    }
}
