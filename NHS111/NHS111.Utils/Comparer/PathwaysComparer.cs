﻿using System.Collections.Generic;
using NHS111.Models.Models.Domain;

namespace NHS111.Utils.Comparer
{
    public class PathwaysComparer : IEqualityComparer<GroupedPathways>
    {

        public bool Equals(GroupedPathways x, GroupedPathways y)
        {
            return x.Group == y.Group;
        }

        public int GetHashCode(GroupedPathways obj)
        {
            return obj.Group.GetHashCode();
        }
    }
}