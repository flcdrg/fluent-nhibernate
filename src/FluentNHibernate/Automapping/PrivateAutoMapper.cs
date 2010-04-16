﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class PrivateAutoMapper : AutoMapper
    {
        private readonly AutoMappingExpressions localExpressions;

        internal PrivateAutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder, IEnumerable<InlineOverride> inlineOverrides)
            : base(expressions, conventionFinder, inlineOverrides)
        {
            localExpressions = expressions;
        }

        public override void ProcessClass(ClassMappingBase mapping, Type entityType, IList<Member> mappedMembers)
        {
            // This will ONLY map private properties. Do not call base.

            var rule = localExpressions.FindMappablePrivateProperties;
            if (rule == null)
                throw new InvalidOperationException("The FindMappablePrivateProperties convention must be supplied to use the PrivateAutoMapper. ");

            foreach (var property in entityType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Select(x => x.ToMember()))
            {
                if (rule(property))
                    TryToMapProperty(mapping, property, mappedMembers);
            }
        }
    }
}
