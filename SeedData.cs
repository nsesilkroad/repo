// <copyright file="SeedData.cs" company="NSE">
// COPYRIGHT © 2014 NSE Inc. ALL RIGHTS RESERVED.
// </copyright>
// <author>wrb</author>
// <date>2018-12-21</date>

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Silkroad.Core.Base.Const.TemplateTrackerType;
using Silkroad.Core.Base.Enumeration.ExtTypes;
using Silkroad.Core.Base.Model.CustomFields;
using Silkroad.Modules.ComplianceManagement.Model;

namespace Silkroad.Modules.ComplianceManagement.Data
{
    /// <summary>
    ///     기본 데이터베이스 Seed Data 관리 클래스
    /// </summary>
    public class SeedData : ISeedData
    {
        /// <summary>
        ///     Compliance Tracker 데이터에 대한 기본 TrackerType 데이터
        /// </summary>
        private static readonly TrackerType ComplianceTrackerData = new TrackerType()
        {
            Name = nameof(Compliance),
            DisplayName = nameof(Compliance),
            TargetModuleTypeName = typeof(Compliance).FullName,
            Description = $"Template Tracker - {nameof(Compliance)}",
            DataCategory = EDataCategory.Template,
            ExtDatas = new Dictionary<string, object>()
            {
                { "Filter" , "Package" },
                { "UserCreatable" , true },
                { "IsMulti" , false },
                { "IsSearch" , false }
            }
        }.CreateFields(tracker =>
        {
            tracker.Fields = new List<FieldType>
            {
                tracker.CreateDefaultFieldType("Name", "Name", EDataType.String, isRequired: true)
                    .SetFieldCategory(new FieldTypeConfig
                    {
                        IsForm = true,
                        IsColumn = true
                    })
                    .SetCustomConfig(field => 
                    {
                        field.IsAllowBlank = false;
                        field.DefaultValue = "New Document";
                    }),
                tracker.CreateDefaultFieldType("FileName", "FileName", EDataType.String),
                tracker.CreateDefaultFieldType("ComplianceCategory", "ComplianceCategory", EDataType.Integer),
                tracker.CreateDefaultFieldType("TagName", "TagName", EDataType.String),
                tracker.CreateDefaultFieldType("index", "index", EDataType.Integer, "Index"),
                tracker.CreateDefaultFieldType("Content", "Description", EDataType.Text, "Description")
                    .SetFieldCategory(new FieldTypeConfig
                    {
                        IsForm = true,
                        IsEditable = true
                    })
            };
        });

        /// <summary>
        ///     Compliance Template Tracker Data Return
        /// </summary>
        /// <returns></returns>
        public List<TrackerType> TemplateList()
        {
            return new List<TrackerType>() { ComplianceTrackerData.CloneForTemplateType() as TrackerType };
        }

        /// <summary>
        ///     기본 데이터베이스 Seed Data 관리 클래스
        /// </summary>
        /// <param name="app"></param>
        public static void InitializeComplianceManagementDb<T>(IApplicationBuilder app)
            where T : IComplianceManagementDbContext
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<T>();

                InitializeComplianceManagementDb(context);
            }
        }

        /// <summary>
        ///     기본 데이터베이스 Seed Data 관리 클래스
        /// </summary>
        /// <param name="context"></param>
        public static void InitializeComplianceManagementDb(IComplianceManagementDbContext context)
        {
            context.Database.Migrate();

            // User 데이터의 기본 데이터를 입력
            if (!context.Compliances.Any())
            {
                var initialDatas = GetDefaultCompliance();

                if (initialDatas != null)
                {
                    context.Compliances.AddRange();
                    context.SaveChanges();
                }
            }

            // template 데이터 생성
            if (!context.ComplianceTemplates.Any())
            {
                InitializeComplianceTemplateDatas(context);
            }
        }

        /// <summary>
        ///     기본 User 데이터를 가져옴
        /// </summary>
        /// <returns></returns>
        private static List<Compliance> GetDefaultCompliance()
        {
            return null;
        }

        /// <summary>
        ///     기본 Template 데이터 생성
        /// </summary>
        /// <param name="context"></param>
        public static void InitializeComplianceTemplateDatas(IComplianceManagementDbContext context)
        {
            if (!context.ComplianceTemplates.Any())
            {
                #region Phase

                var aerospacePhaseList = new List<ComplianceTemplate>();
                var nuclearPhaseList = new List<ComplianceTemplate>();
                var defensePhaseList = new List<ComplianceTemplate>();
                var automotivePhaseList = new List<ComplianceTemplate>();
                var medicalPhaseList = new List<ComplianceTemplate>();
                var railwayPhaseList = new List<ComplianceTemplate>();

                var aerospace_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Planning", TagName = "select_0001" }; aerospacePhaseList.Add(aerospace_1);
                var aerospace_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Requirement", TagName = "select_0002" }; aerospacePhaseList.Add(aerospace_2);
                var aerospace_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Design", TagName = "select_0003" }; aerospacePhaseList.Add(aerospace_3);
                var aerospace_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Coding", TagName = "select_0004" }; aerospacePhaseList.Add(aerospace_4);
                var aerospace_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Integration", TagName = "select_0005" }; aerospacePhaseList.Add(aerospace_5);
                var aerospace_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Verification", TagName = "select_0006" }; aerospacePhaseList.Add(aerospace_6);
                var aerospace_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Configuration Management", TagName = "select_0007" }; aerospacePhaseList.Add(aerospace_7);
                var aerospace_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Quality Assurance", TagName = "select_0008" }; aerospacePhaseList.Add(aerospace_8);
                var aerospace_9 = new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Aerospace, Name = "Certification Liaison", TagName = "select_0009" }; aerospacePhaseList.Add(aerospace_9);

                var nuclear_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System requirements specification", TagName = "select_0001" }; nuclearPhaseList.Add(nuclear_1);
                var nuclear_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System specification", TagName = "select_0002" }; nuclearPhaseList.Add(nuclear_2);
                var nuclear_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System detailed design and implementation", TagName = "select_0003" }; nuclearPhaseList.Add(nuclear_3);
                var nuclear_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System integration", TagName = "select_0004" }; nuclearPhaseList.Add(nuclear_4);
                var nuclear_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System validation", TagName = "select_0005" }; nuclearPhaseList.Add(nuclear_5);
                var nuclear_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System installation", TagName = "select_0006" }; nuclearPhaseList.Add(nuclear_6);
                var nuclear_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System design modifications", TagName = "select_0007" }; nuclearPhaseList.Add(nuclear_7);
                var nuclear_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Nuclear, Name = "System planning", TagName = "select_0008" }; nuclearPhaseList.Add(nuclear_8);

                var defense_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, Name = "개발 준비", TagName = "" }; defensePhaseList.Add(defense_1);
                var defense_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, Name = "체계 요구사항분석", TagName = "select_0001" }; defensePhaseList.Add(defense_2);
                var defense_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, Name = "체계 구조설계", TagName = "select_0002" }; defensePhaseList.Add(defense_3);
                var defense_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어 요구사항분석", TagName = "select_0003" }; defensePhaseList.Add(defense_4);
                var defense_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어 구조설계", TagName = "select_0004" }; defensePhaseList.Add(defense_5);
                var defense_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어 상세설계", TagName = "select_0005" }; defensePhaseList.Add(defense_6);
                var defense_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어 구현", TagName = "select_0006" }; defensePhaseList.Add(defense_7);
                var defense_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어 통합 및 시험", TagName = "select_0007" }; defensePhaseList.Add(defense_8);
                var defense_9 = new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Defense, Name = "체계통합 및 시험", TagName = "select_0008" }; defensePhaseList.Add(defense_9);
                var defense_10 = new ComplianceTemplate() { Index = 9, ComplianceCategory = EComplianceCategoryType.Defense, Name = "개발시험평가", TagName = "select_0009" }; defensePhaseList.Add(defense_10);
                var defense_11 = new ComplianceTemplate() { Index = 10, ComplianceCategory = EComplianceCategoryType.Defense, Name = "운용시험평가", TagName = "select_0010" }; defensePhaseList.Add(defense_11);
                var defense_12 = new ComplianceTemplate() { Index = 11, ComplianceCategory = EComplianceCategoryType.Defense, Name = "소프트웨어설치", TagName = "" }; defensePhaseList.Add(defense_12);
                var defense_13 = new ComplianceTemplate() { Index = 12, ComplianceCategory = EComplianceCategoryType.Defense, Name = "규격화", TagName = "select_0011" }; defensePhaseList.Add(defense_13);
                var defense_14 = new ComplianceTemplate() { Index = 13, ComplianceCategory = EComplianceCategoryType.Defense, Name = "인도", TagName = "select_0011" }; defensePhaseList.Add(defense_14);

                var automotive_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "4.7 System design", TagName = "select_0001" }; automotivePhaseList.Add(automotive_1);
                var automotive_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.5 Initiation of product development at the SW level", TagName = "select_0002" }; automotivePhaseList.Add(automotive_2);
                var automotive_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.6 Specification of SW safety requirements", TagName = "select_0003" }; automotivePhaseList.Add(automotive_3);
                var automotive_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.7 SW architectural design", TagName = "select_0004" }; automotivePhaseList.Add(automotive_4);
                var automotive_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.8 SW unit design and implementation", TagName = "select_0005" }; automotivePhaseList.Add(automotive_5);
                var automotive_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.9 SW unit testing", TagName = "select_0006" }; automotivePhaseList.Add(automotive_6);
                var automotive_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.10 SW unit integration and testing", TagName = "select_0007" }; automotivePhaseList.Add(automotive_7);
                var automotive_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "6.11 Verification of SW safety requirements", TagName = "select_0008" }; automotivePhaseList.Add(automotive_8);
                var automotive_9 = new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "4.8 Item integration and testing", TagName = "select_0009" }; automotivePhaseList.Add(automotive_9);
                var automotive_10 = new ComplianceTemplate() { Index = 9, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "8. Supporting process", TagName = "select_0010" }; automotivePhaseList.Add(automotive_10);
                var automotive_11 = new ComplianceTemplate() { Index = 10, ComplianceCategory = EComplianceCategoryType.Automotive, Name = "9. ASIL-oriented and safety-oriented analyses", TagName = "select_0011" }; automotivePhaseList.Add(automotive_11);

                var medical_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.1 소프트웨어 개발 기획", TagName = "select_0001" }; medicalPhaseList.Add(medical_1);
                var medical_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.2 소프트웨어 요구사항 분석", TagName = "select_0002" }; medicalPhaseList.Add(medical_2);
                var medical_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.3 소프트웨어 구축 설계", TagName = "select_0003" }; medicalPhaseList.Add(medical_3);
                var medical_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.4 소프트웨어 상세 설계", TagName = "select_0004" }; medicalPhaseList.Add(medical_4);
                var medical_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.5 소프트웨어 장비구현 및 검증", TagName = "select_0005" }; medicalPhaseList.Add(medical_5);
                var medical_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.6 서브시스템 통합 및 통합 시험", TagName = "select_0006" }; medicalPhaseList.Add(medical_6);
                var medical_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.7 소프트웨어 시스템 시험", TagName = "select_0007" }; medicalPhaseList.Add(medical_7);
                var medical_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Medical, Name = "5.8 소프트웨어 개발 기획", TagName = "select_0008" }; medicalPhaseList.Add(medical_8);
                var medical_9 = new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Medical, Name = "7. 소프트웨어 위험관리", TagName = "select_0009" }; medicalPhaseList.Add(medical_9);
                var medical_10 = new ComplianceTemplate() { Index = 9, ComplianceCategory = EComplianceCategoryType.Medical, Name = "8. 소프트웨어 형상관리", TagName = "select_0010" }; medicalPhaseList.Add(medical_10);
                var medical_11 = new ComplianceTemplate() { Index = 10, ComplianceCategory = EComplianceCategoryType.Medical, Name = "9. 소프트웨어 문제 해결", TagName = "select_0011" }; medicalPhaseList.Add(medical_11);

                var railway_1 = new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, Name = "System Inputs", TagName = "select_0001" }; railwayPhaseList.Add(railway_1);
                var railway_2 = new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software Planning", TagName = "select_0002" }; railwayPhaseList.Add(railway_2);
                var railway_3 = new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software Requirements", TagName = "select_0003" }; railwayPhaseList.Add(railway_3);
                var railway_4 = new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software Design", TagName = "select_0004" }; railwayPhaseList.Add(railway_4);
                var railway_5 = new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software Module Design", TagName = "select_0005" }; railwayPhaseList.Add(railway_5);
                var railway_6 = new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Code", TagName = "select_0006" }; railwayPhaseList.Add(railway_6);
                var railway_7 = new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Module Testing", TagName = "select_0007" }; railwayPhaseList.Add(railway_7);
                var railway_8 = new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software Integration", TagName = "select_0008" }; railwayPhaseList.Add(railway_8);
                var railway_9 = new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Software/Hardware Integration", TagName = "select_0009" }; railwayPhaseList.Add(railway_9);
                var railway_10 = new ComplianceTemplate() { Index = 9, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Validation", TagName = "select_0010" }; railwayPhaseList.Add(railway_10);
                var railway_11 = new ComplianceTemplate() { Index = 10, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Assessment", TagName = "select_0011" }; railwayPhaseList.Add(railway_11);
                var railway_12 = new ComplianceTemplate() { Index = 11, ComplianceCategory = EComplianceCategoryType.Railway, Name = "Maintenance", TagName = "select_0012" }; railwayPhaseList.Add(railway_12);

                context.ComplianceTemplates.AddRange(aerospacePhaseList);
                context.ComplianceTemplates.AddRange(nuclearPhaseList);
                context.ComplianceTemplates.AddRange(defensePhaseList);
                context.ComplianceTemplates.AddRange(automotivePhaseList);
                context.ComplianceTemplates.AddRange(medicalPhaseList);
                context.ComplianceTemplates.AddRange(railwayPhaseList);

                context.SaveChanges();

                #endregion

                #region Document

                var templateList = new List<ComplianceTemplate>();

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Plan for Software Aspects of Certification", FileName = "Template_Aerospace_01_PSAC", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Development Plan", FileName = "Template_Aerospace_02_SDP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Verification Plan", FileName = "Template_Aerospace_03_SVP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Configuration Management Plan", FileName = "Template_Aerospace_04_SCMP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Quality Assurance Plan", FileName = "Template_Aerospace_05_SQAP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Requirements Standards", FileName = "Template_Aerospace_06_SRStd", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Design Standards", FileName = "Template_Aerospace_07_SDStd", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Code Standards", FileName = "Template_Aerospace_08_SCStd", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_1, Name = "Software Verification Results", FileName = "Template_Aerospace_14_SVR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_2, Name = "Software Requirements Data", FileName = "Template_Aerospace_09_SRD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_2, Name = "Trace Data", FileName = "Template_Aerospace_21_SoftwareRequirementsTraceMatrix", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_2, Name = "Software Verification Results(Refined)", FileName = "Template_Aerospace_14_SVR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_3, Name = "Design Description", FileName = "Template_Aerospace_10_SDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_3, Name = "Trace Data(Refined)", FileName = "Template_Aerospace_21_SoftwareRequirementsTraceMatrix", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_3, Name = "Software Verification Results(Refined)", FileName = "Template_Aerospace_14_SVR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_4, Name = "Source Code", FileName = "Template_Aerospace_11_SourceCode", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_4, Name = "Trace Data(Refined)", FileName = "Template_Aerospace_21_SoftwareRequirementsTraceMatrix", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_4, Name = "Software Verification Results(Refined)", FileName = "Template_Aerospace_14_SVR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_5, Name = "Executable Object Code", FileName = "Template_Aerospace_12_ExecutableObjectCode", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_5, Name = "Parameter Data Item File", FileName = "Template_Aerospace_22_ParameterDataItemFile", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_5, Name = "Software Verification Results(Refined)", FileName = "Template_Aerospace_14_SVR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_6, Name = "Software Verification Cases and Procedures", FileName = "Template_Aerospace_13_SVCP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_6, Name = "Software Verification Results(Refined)", FileName = "Template_Aerospace_14_SVR", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_6, Name = "Trace Data", FileName = "Template_Aerospace_21_SoftwareRequirementsTraceMatrix", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_7, Name = "SCM Records", FileName = "Template_Aerospace_18_SCMR", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_7, Name = "Software Configuration Index", FileName = "Template_Aerospace_16_SCI", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_7, Name = "Problem Reports", FileName = "Template_Aerospace_17_PR", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_7, Name = "Software Life Cycle Environment Configuration Index", FileName = "Template_Aerospace_15_SECI", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_8, Name = "SQA Records", FileName = "Template_Aerospace_19_SQAR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Aerospace, ParentComplianceTemplate = aerospace_9, Name = "Software Accomplishment Summary", FileName = "Template_Aerospace_20_SAS", TagName = null });



                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_1, Name = "System requirements specification", FileName = "Template_NPP_01_SystemRequirementSpecification", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_2, Name = "System specification documentation", FileName = "Template_NPP_02_SystemSpecificationDocumentation", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_3, Name = "System detailed design documentation", FileName = "Template_NPP_03_SystemDetailedDocumentation", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_4, Name = "Integration report", FileName = "Template_NPP_04_IntegrationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_5, Name = "System validation report", FileName = "Template_NPP_05_SystemValidationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_6, Name = "Installation report", FileName = "Template_NPP_06_SystemInstallationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_7, Name = "Modification reports", FileName = "Template_NPP_07_SystemModificationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Quality Assurance Plan", FileName = "Template_NPP_00_SystemQualityAssuranceplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Security Plan", FileName = "Template_NPP_00_SystemSecurityPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Integration Plan", FileName = "Template_NPP_00_SystemIntegrationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Validation Plan", FileName = "Template_NPP_00_SystemValidationPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Installation Plan", FileName = "Template_NPP_00_SystemInstallationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System Operation Plan", FileName = "Template_NPP_00_SystemOperationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Nuclear, ParentComplianceTemplate = nuclear_8, Name = "System maintenance plan", FileName = "Template_NPP_00_SystemMaintenanceplan", TagName = null });



                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_1, Name = "탐색/체계개발실행계획서", FileName = "SW기술문서-00_체계개발실행계획서_SDIP", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_2, Name = "체계요구사항명세서", FileName = "SW기술문서-00_체계요구사항명세서_SSRS", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_2, Name = "소프트웨어개발계획서(체계개발실행계획서에포함 가능)", FileName = "SW기술문서-01_소프트웨어개발계획서_SDP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_2, Name = "MND-AF 산출물(AV, OV)", FileName = "SW기술문서-00_MND_AF산출물_AV_OV", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_3, Name = "체계설계기술서", FileName = "SW기술문서-00_체계설계기술서_SSDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_3, Name = "MND-AF 산출물(SV, TV)", FileName = "SW기술문서-00_MND_AF산출물_SV_TV", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_4, Name = "소프트웨어요구사항명세서", FileName = "SW기술문서-02_소프트웨어요구사항명세서_SRS", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_5, Name = "(개략)소프트웨어설계기술서", FileName = "SW기술문서-03_소프트웨어설계기술서_SDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_5, Name = "(개략)인터페이스설계기술서((개략)소프트웨어설계기술서에 포함 가능)", FileName = "SW기술문서-04_인터페이스설계기술서_IDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_5, Name = "(개략)데이터베이스설계기술서((개략)데이터베이스설계기술서에 포함 가능)", FileName = "SW기술문서-05_데이터베이스설계기술서_DBDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_5, Name = "MND-AF 산출물(갱신)", FileName = "SW기술문서-04_인터페이스설계기술서_IDD", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "(상세)소프트웨어설계기술서", FileName = "SW기술문서-03_소프트웨어설계기술서_SDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "(상세)인터페이스설계기술서((상세)인터페이스설계기술서에 포함 가능)", FileName = "SW기술문서-04_인터페이스설계기술서_IDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "(상세)데이터베이스설계기술서((상세)데이터베이스설계기술서에 포함 가능)", FileName = "SW기술문서-05_데이터베이스설계기술서_DBDD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "인터페이스통제문서", FileName = "SW기술문서-00_인터페이스통제문서_ICD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "MND-AF 산출물", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "소프트웨어단위시험계획", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_6, Name = "소프트웨어통합시험계획서(초안)", FileName = "SW기술문서-06_소프트웨어통합시험계획서_STP", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_7, Name = "소스코드/실행 프로그램 코드(라이브러리/오브젝트코드 포함)", FileName = "SW기술문서-00_소스코드_실행프로그램코드", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_7, Name = "소프트웨어단위시험결과", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_7, Name = "소프트웨어통합시험계획서", FileName = "SW기술문서-06_소프트웨어통합시험계획서_STP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_7, Name = "소프트웨어통합시험절차서", FileName = "SW기술문서-07_소프트웨어통합시험절차서_STD", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "소프트웨어통합시험결과서", FileName = "SW기술문서-08_소프트웨어통합시험결과서_STR", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "사용자/관리자 문서(초안)", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "- 사용자지침서", FileName = "SW기술문서-00_사용자지침서_SUM", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "- 체계운영자지침서", FileName = "SW기술문서-00_체계운영자지침서_SCOM", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "- 소프트웨어버전기술서(소프트웨어산출물명세서에 포함 가능)", FileName = "SW기술문서-11_소프트웨어버전기술서_SVD", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "- 소프트웨어산출물명세서", FileName = "SW기술문서-09_소프트웨어산출물명세서_SPS", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "체계통합시험계획서", FileName = "SW기술문서-00_체계통합시험계획서_SITP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_8, Name = "체계통합시험절차서", FileName = "SW기술문서-00_체계통합시험절차서", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "체계통합시험결과서", FileName = "SW기술문서-00_체계통합시험결과서_SITR", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "소프트웨어설치계획서", FileName = "SW기술문서-00_소프트웨어설치계획서_SIP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "소프트웨어전이계획서", FileName = "SW기술문서-00_소프트웨어전이계획서_STrP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "펌웨어설치절차서(소프트웨어산출물명세서에 포함 가능)", FileName = "SW기술문서-12_펌웨어설치절차서_FIG", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "소프트웨어목록명세서", FileName = "SW기술문서-10_소프트웨어목록명세서_SCS", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "개발시험평가계획서(안)", FileName = "SW기술문서-00_개발시험평가계획안_DTEP", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_9, Name = "개발시험평가절차서", FileName = "SW기술문서-00_개발시험평가절차서", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_10, Name = "개발시험평가결과보고서", FileName = "SW기술문서-00_개발시험평가결과보고서_DTER", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_10, Name = "운용시험평가지원계획서", FileName = "SW기술문서-00_운용시험평가지원계획서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_10, Name = "사용자/관리자 문서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_11, Name = "운용시험평가지원결과서", FileName = "SW기술문서-00_운용시험평가지원결과서", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_12, Name = "소프트웨어설치결과서", FileName = "SW기술문서-00_소프트웨어설치결과서_SIR", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Defense, ParentComplianceTemplate = defense_13, Name = "규격화", FileName = null, TagName = null });



                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "Technical safety concept", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "System design specification", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "Hardware-software interface specification (HSI)", FileName = "Template_Automotive_Hardware-softwareinterfacespecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "Specification of requirements for production, operation, service and decommissioning", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "System verification report (refined)", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_1, Name = "Safety analysis reports resulting from requirement", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_2, Name = "Safety plan (refined)", FileName = "Template_Automotive_Safetyplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_2, Name = "Software verification plan", FileName = "Template_Autmotive_Softwareverificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_2, Name = "Design and coding guidelines for modeling and programming languages", FileName = "Template_Automotive_Designandcodingguidelinesformodelingandprogramminglanguages", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_2, Name = "Tool application guidelines", FileName = "Template_Automotive_Toolapplicationguidelines", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_3, Name = "Software safety requirements specification", FileName = "Template_Automotive_Softwaresafetyrequirementspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_3, Name = "Hardware-software interface specification", FileName = "Template_Automotive_Hardware-softwareinterfacespecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_3, Name = "Software verification plan (refined)", FileName = "Template_Autmotive_Softwareverificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_3, Name = "Software verification report", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Software architectural design specification", FileName = "Template_Autmotive_SoftwareArchitecturalanddesignpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Safety plan (refined)", FileName = "Template_Automotive_Safetyplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Software safety requirements specification (refined)", FileName = "Template_Automotive_Softwaresafetyrequirementspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Safety analysis report", FileName = "Template_Automotive_Safetyanalysispeport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Dependent failures analysis report", FileName = "Template_Automotive_Dependentfailureanalysis", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_4, Name = "Software verification report (refined)", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_5, Name = "Software unit design specification", FileName = "Template_Automotive_Softwareunitdesignspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_5, Name = "Software unit implementation", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_5, Name = "Software verification report (refined)", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_6, Name = "Software verification plan (refined)", FileName = "Template_Autmotive_Softwareverificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_6, Name = "Software verification specification", FileName = "Template_Automotive_Softwareverificationspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_6, Name = "Software verification report (refined)", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_7, Name = "Software verification plan (refined)", FileName = "Template_Autmotive_Softwareverificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_7, Name = "Software verification specification (refined)", FileName = "Template_Automotive_Softwareverificationspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_7, Name = "Embedded software", FileName = "Template_Automotive_Embeddedsoftware", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_7, Name = "Software verification report (refined)", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_8, Name = "Software verification plan (refined)", FileName = "Template_Autmotive_Softwareverificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_8, Name = "Software verification specification (refined)", FileName = "Template_Automotive_Softwareverificationspecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_8, Name = "Software verification report (refined)", FileName = "Template_Autmotive_Softwareverificationreport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_9, Name = "Item integration and testing plan (refined)", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_9, Name = "Integration testing specification", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_9, Name = "Integration testing report", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Supplier selection report", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Development Interface Agreement (DIA)", FileName = "Template_Automotive_Developmentinterfaceagreement(DIA)", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Supplier's project plan", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Supplier's safety plan", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Safety assessment report", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Supply agreement", FileName = "Template_Automotive_Supplyagreement", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Configuration management plan", FileName = "Template_Automotive_Configurationmanagementplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Change management plan", FileName = "Template_Automotive_ChangeManagementPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Change request", FileName = "Template_Automotive_Changerequest", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 9, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Impact analysis & change request plan", FileName = "Template_Automotive_ImpactanalysisandChangerequestplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 10, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Change report", FileName = "Template_Automotive_Changereport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 11, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Verification plan", FileName = "Template_Automotive_Verification_Confirmationreview", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 12, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Specification of verification", FileName = "Template_Automotive_Verification_Confirmationreview", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 13, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Verification report", FileName = "Template_Automotive_Verification_Confirmationreview", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 14, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Document management plan", FileName = "Template_Automotive_Documentmanagementplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 15, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Documentation guideline requirements", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 16, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Software tool criteria evaluation report", FileName = "Template_Automotive_Softwaretoolcriteriaevaluationreport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 17, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Software tool qualification report", FileName = "Template_Automotive_Softwaretoolqualificationreport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 18, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Software component documentation", FileName = "Template_Automotive_Softwarecomponentdocumentation", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 19, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Software component qualification report", FileName = "Template_Automotive_Softwarecomponentqualificationreport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 20, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Qualification plan", FileName = "Template_Automotive_Qualificationplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 21, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Hardware component test plan", FileName = "Template_Automotive_Hardwarecomponenttestplan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 22, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Qualification report", FileName = "Template_Automotive_Qualificationreport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 23, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Definition of candidate for proven in use argument", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 24, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_10, Name = "Proven in use analysis reports", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_11, Name = "Update of architectural information", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_11, Name = "Update of ASIL as attribute of safety requirements and elements", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_11, Name = "Update of ASIL as attribute of sub-elements of elements", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_11, Name = "Analysis of dependent failures", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Automotive, ParentComplianceTemplate = automotive_11, Name = "Safety analyses", FileName = null, TagName = null });



                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "프로젝트 계획서", FileName = "프로젝트계획서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "진척 보고서", FileName = "진척보고서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "개발방법론", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "위험관리 계획서", FileName = "위험관리계획서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "표준 프로세스", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "자원관리 계획서", FileName = "자원관리계획서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_1, Name = "생명주기", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_2, Name = "요구사항 정의서", FileName = "요구사항정의서", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_2, Name = "요구사항변경기록", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_2, Name = "요구사항 분석서", FileName = "요구사항분석서", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_3, Name = "SW 구조 설계서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "구조설계산출물", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "화면명세서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "프로그램명세서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "DB설계서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "인터페이스설계서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_4, Name = "상세설계서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_5, Name = "코딩표준", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_5, Name = "소스코드검토서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_5, Name = "단위테스트시나리오", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_5, Name = "단위테스트케이스", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_6, Name = "SW 통합 계획서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_6, Name = "테스트결과서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_6, Name = "통합테스트계획서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_6, Name = "통합테스트결과서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_7, Name = "시스템 테스트 결과서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_8, Name = "소프트웨어 검증 확인서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_8, Name = "소프트웨어 배포 결과", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_9, Name = "영향분석 보고서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_9, Name = "위험관리 요구사항", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_9, Name = "위험관리 보고서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_9, Name = "변경분석보고서", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_10, Name = "형상항목", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_10, Name = "형상관리계획서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_10, Name = "형상관리계획서", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Medical, ParentComplianceTemplate = medical_10, Name = "형상항목변경관리 대장", FileName = null, TagName = null });



                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_1, Name = "System Requirements Specification", FileName = "Template_Railway_00_SystemRequirementSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_1, Name = "System Safety Requirements Specification", FileName = "Template_Railway_00_SystemSafetyRequirementSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_1, Name = "System Architecture Description", FileName = "Template_Railway_00_SystemArchitectureDescription", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_1, Name = "System Safety Plan", FileName = "Template_Railway_00_SystemSafetyPlan", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Quality Assurance Plan", FileName = "Template_Railway_01_SoftwareQualityAssurancePlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Configuration Management Plan", FileName = "Template_Railway_00_SoftwareConfigurationManagementPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Verification Plan", FileName = "Template_Railway_01_SoftwareVerificationPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 3, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Integration Test Plan", FileName = "Template_Railway_00_SoftwareIntegrationTestPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 4, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software/Hardware Integration Test Plan", FileName = "Template_Railway_00_SoftwareHardwareIntegrationTestPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 5, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Validation Plan", FileName = "Template_Railway_01_SoftwareValidationPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 6, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Maintenance Plan", FileName = "Template_Railway_01_SoftwareMaintenancePlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 7, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Development Plan", FileName = "Template_Railway_00_SoftwareDevelopmentPlan", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 8, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_2, Name = "Software Integration Plan", FileName = "Template_Railway_01_SoftwareIntegrationPlan", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_3, Name = "Software Requirements Specification", FileName = "Template_Railway_02_SoftwareRequirementSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_3, Name = "Software Requirements Test Specification", FileName = "Template_Railway_00_SoftwareRequirementTestSpec", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_3, Name = "Software Requirements Verification Report", FileName = "Template_Railway_00_SoftwareRequirementVerificationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_4, Name = "Software Architecture Specification", FileName = "Template_Railway_00_SoftwareArchitectureSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_4, Name = "Software Design Specification", FileName = "Template_Railway_03_SoftwareDesignSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_4, Name = "Software Arch. And Design Verification Report", FileName = "Template_Railway_00_SoftwareArchDesignVerificationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_5, Name = "Software Module Design Specification", FileName = "Template_Railway_00_SoftwareModuleDesignSpecification", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_5, Name = "Software Module Test Specification", FileName = "Template_Railway_00_SoftwareModuleTestSpec", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 2, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_5, Name = "Software Module Verification Report", FileName = "Template_Railway_00_SoftwareModuleVerificationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_6, Name = "Software Source Code", FileName = null, TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 1, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_6, Name = "Software Source Code Verification Report", FileName = "Template_Railway_00_SoftwareSourceCodeVerificationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_7, Name = "Software Module Test Report", FileName = "Template_Railway_00_SoftwareModuleTestReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_8, Name = "Software Integration Test Report", FileName = "Template_Railway_00_SoftwareIntegrationTestReport", TagName = null });
                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_8, Name = "Data Test Report", FileName = null, TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_9, Name = "Software/Hardware Integration Test Report", FileName = "Template_Railway_00_SoftwareHardwareIntegrationTestReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_10, Name = "Software Validation Report", FileName = "Template_Railway_04_SoftwareValidationReport", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_11, Name = "Software Change Records", FileName = "Template_Railway_00_SoftwareChangeRecords", TagName = null });

                templateList.Add(new ComplianceTemplate() { Index = 0, ComplianceCategory = EComplianceCategoryType.Railway, ParentComplianceTemplate = railway_12, Name = "Software Maintenance Records", FileName = "Template_Railway_00_SoftwaremaintenanceRecords", TagName = null });


                context.ComplianceTemplates.AddRange(templateList);
                context.SaveChanges();

                #endregion

            }
        }
    }
}