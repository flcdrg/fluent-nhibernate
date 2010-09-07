﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Diagnostics
{
    public class DefaultOutputFormatter : IDiagnosticResultsFormatter
    {
        public string Format(DiagnosticResults results)
        {
            var sb = new StringBuilder();

            OutputFluentMappings(sb, results);

            sb.AppendLine();

            OutputConventions(sb, results);

            return sb.ToString();
        }

        void OutputConventions(StringBuilder sb, DiagnosticResults results)
        {
            Title(sb, "Conventions");

            sb.AppendLine();
            sb.AppendLine("Sources scanned:");
            sb.AppendLine();

            var sources = results.ScannedSources
                    .Where(x => x.Phase == ScanPhase.Conventions)
                    .OrderBy(x => x.Identifier)
                    .Select(x => x.Identifier)
                    .ToArray();

            if (sources.Any())
            {
                List(sb, sources);

                sb.AppendLine();
                sb.AppendLine("Types discovered:");
                sb.AppendLine();

                if (results.Conventions.Any())
                {
                    var conventions = results.Conventions
                        .OrderBy(x => x.Name)
                        .ToArray();

                    Table(sb,
                        conventions.Select(x => x.Name),
                        conventions.Select(x => x.AssemblyQualifiedName));
                }
                else
                {
                    sb.AppendLine("  None found");
                }
            }
            else
            {
                sb.AppendLine("  None found");
            }
        }

        void OutputFluentMappings(StringBuilder sb, DiagnosticResults results)
        {
            Title(sb, "Fluent Mappings");
            
            sb.AppendLine();
            sb.AppendLine("Sources scanned:");
            sb.AppendLine();

            var sources = results.ScannedSources
                    .Where(x => x.Phase == ScanPhase.FluentMappings)
                    .OrderBy(x => x.Identifier)
                    .Select(x => x.Identifier)
                    .ToArray();

            if (sources.Any())
            {
                List(sb, sources);

                sb.AppendLine();
                sb.AppendLine("Types discovered:");
                sb.AppendLine();

                if (results.FluentMappings.Any())
                {
                    var fluentMappings = results.FluentMappings
                        .OrderBy(x => x.Name)
                        .ToArray();

                    Table(sb,
                        fluentMappings.Select(x => x.Name),
                        fluentMappings.Select(x => x.AssemblyQualifiedName));
                }
                else
                {
                    sb.AppendLine("  None found");
                }
            }
            else
            {
                sb.AppendLine("  None found");
            }
        }

        void Table(StringBuilder sb, params IEnumerable<string>[] columns)
        {
            var columnWidths = columns
                .Select(x => x.Max(val => val.Length))
                .ToArray();
            var rowCount = columns.First().Count();

            for (var row = 0; row < rowCount; row++)
            {
                sb.Append("  ");
                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    var width = columnWidths[i];
                    var value = column.ElementAt(row);

                    sb.Append(value.PadRight(width));
                    sb.Append(" | ");
                }

                sb.Length -= 3; // remove last separator
                sb.AppendLine();
            }
        }

        void List(StringBuilder sb, IEnumerable<string> items)
        {
            foreach (var item in items)
                sb.AppendLine("  " + item);
        }

        void Title(StringBuilder sb, string title)
        {
            sb.AppendLine(title);
            sb.AppendLine("".PadLeft(title.Length, '-'));
        }
    }
}