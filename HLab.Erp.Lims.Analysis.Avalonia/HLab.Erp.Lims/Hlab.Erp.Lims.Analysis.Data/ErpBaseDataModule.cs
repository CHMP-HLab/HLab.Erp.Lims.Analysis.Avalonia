﻿using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Data;

public class HLabErpLimsAnalysisDataUpdaterBootloader : DataUpdaterBootloader
{
    public HLabErpLimsAnalysisDataUpdaterBootloader(IDataService data) : base(data)
    { }

    protected override ISqlBuilder GetSqlUpdater(string version, ISqlBuilder builder)
    {
        switch (version)
        {
            case "0.0.0.0":
                builder
                        .Table<FormClass>().Create()
                        .Table<SampleForm>().Create()

                        .Table<Sample>()
                            .AddColumn(s => s.CustomerReference)
                            .AddColumn(s => s.ReportReference)

                        .Table<SampleTest>()
                            .AddColumn(st => st.PharmacopoeiaId)
                            .AddColumn(st => st.PharmacopoeiaVersion)

                        .Table<LocalizeEntry>().AddColumn(s => s.Custom)
                        
                        .Version("1.0.0.0")
                        ;
                    break;
                case "1.0.0.0":
                    builder
                    .Table<Form>()
                        .AddColumn(f => f.IconPath)
                    .Table<LinkedDocument>()
                        .Create()
                    .Table<AnalysisMotivation>()
                        .Create()
                    .Table<Sample>()
                        .AddColumn(s => s.AnalysisMotivation)
                        .RenameColumn("State", s => s.ConformityId)
                    .Table<SampleTestResult>()
                        .RenameColumn("StateId", s => s.ConformityId)
                    .Table<Pharmacopoeia>()
                        .RenameColumn("NameEn", s => s.Name)
                        .AddColumn(p => p.IconPath)
                    .Table<Icon>()
                        .AddColumn(i => i.Foreground)
                    .Version("2.0.0.0")
                    ;
                break;
            case "2.0.0.0":
                builder
                    .Table<TestClassUnitTest>()
                        .AddColumn(t => t.TestClass)
                        .RenameColumn("AssayName", t=> t.TestName)
                        .RenameColumn("Specifications", t=> t.Specification)
                        .AddColumn(t=> t.Name)
                        .AddColumn(t=> t.SpecificationValues)
                        .AddColumn(t=> t.SpecificationDone)
                        .AddColumn(t=> t.ConformityId)
                        .AddColumn(t=> t.Conformity)
                        .AddColumn(t=> t.MandatoryDone)
                    .Table<SampleForm>()
                        .RenameColumn("SpecificationsDone", t=> t.SpecificationDone)
                        .RenameColumn("SpecValues", t=> t.SpecificationValues)
                        .RenameColumn("Values", t=> t.ResultValues)
                    .Table<FormClass>()
                        .AddColumn(t=> t.CodeHash)
                    .Table<Continent>()
                        .AddColumn(c => c.Code)
                    .Version("2.1.0.0");
                break;
            case "2.1.0.0":
                builder
                    .Table<SampleTest>()
                    .RenameColumn("SpecificationsDone", t => t.SpecificationDone)

                    .Table<SampleForm>()
                    .RenameColumn("State", s => s.ConformityId)

                    .Table<TestClassUnitTest>()
                        .RenameColumn("Values", t => t.ResultValues)

                        .Table<Sample>().AddColumn(t => t.PreviousStageId)

                        .Table<Sample>().AddColumn(t => t.Progress)
                        .Table<SampleTest>().AddColumn(t => t.Progress)
                        .Table<SampleTestResult>().AddColumn(t => t.Progress)
                        .Version("2.2.0.0");
                         break;
                        ;
                case "2.2.0.0":
                    builder
                        .Table<ProductCategory>()
                        .AddColumn(t => t.NamePropertyName)
                        .AddColumn(t => t.VariantPropertyName)
                        .AddColumn(t => t.ComplementPropertyName)
                        .Table<Product>()
                            .RenameColumn("Inn",t=>t.Name)
                            .RenameColumn("Dose",t=>t.Variant)

                        .Version("2.3.0.0");
                        ;
                    break;
                case "2.3.0.0":
                    builder
                        .Table<StatQuery>()
                            .RenamedFrom("Requete")
                            .RenameColumn("Requete",t=>t.Query)

                        .Version("2.4.0.0");
                        ;
                    break;
                case "2.4.0.0":
                    builder
                        .Table<Sample>()
                            .RenameColumn("Stage",t=>t.StageId)

                        .Version("2.5.0.0");
                        ;
                    break;
                case "2.5.0.0":
                    builder
                        .Table<ProductComponent>()
                            .Create()
                        .Table<Inn>()
                            .Create()
                            .AddColumn(t => t.CasNumber)
                            .AddColumn(t => t.Density)
                            .AddColumn(t => t.Caption)
                            .AddColumn(t => t.MolarMass)
                            .AddColumn(t => t.UnitGroup)
                        .Table<SampleMovementMotivation>().Create()
                        .Table<SampleMovement>().Create()
                        .Table<TestClass>().AddColumn(t => t.Binary)
                        .Table<TestClass>().AddColumn(c => c.ComponentAware)
                        .Table<SampleTest>().AddColumn(c => c.ProductComponentId)
                        .Version("2.5.0.1");
                        ;
                    break;
                case "2.5.0.1":
                    builder.Table<SampleMovement>().AddColumn(s => s.Date);
                    builder.Table<Sample>().AddColumn(s => s.RemainingQuantity);
                break;

        }
        return base.GetSqlUpdater(version, builder);
    }
}
