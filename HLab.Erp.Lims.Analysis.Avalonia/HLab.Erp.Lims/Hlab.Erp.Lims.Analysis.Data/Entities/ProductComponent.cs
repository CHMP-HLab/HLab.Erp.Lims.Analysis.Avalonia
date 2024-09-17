using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data.Entities
{
    public class ProductComponent : Entity, IListableModel
    {
        public ProductComponent()
        {
            _unit = Foreign(this, e => e.UnitId, e => e.Unit);
            _product = Foreign(this, e => e.ProductId, e => e.Product);
            _inn = Foreign(this, e => e.InnId, e => e.Inn);
        }

        public string Caption => $"{Inn?.Caption} {Quantity,0} {Unit?.Symbol}";

        public int? ProductId
        {
            get => _product.Id;
            set => _product.SetId(value);
        }
        [Ignore]
        public Product Product
        {
            get => _product.Value;
            set => ProductId = value.Id;
        }

        readonly ForeignPropertyHelper<ProductComponent, Product> _product;

        public int? InnId
        {
            get => _inn.Id;
            set => _inn.SetId(value);
        }
        [Ignore]
        public Inn Inn
        {
            get => _inn.Value;
            set => InnId = value?.Id;
        }
        readonly ForeignPropertyHelper<ProductComponent, Inn> _inn;

        public double Quantity
        {
            get => _quantity;
            set => SetAndRaise(ref _quantity, value);
        }
        double _quantity = 0.0;

        public int? UnitId
        {
            get => _unit.Id;
            set => _unit.SetId(value);
        }
        [Ignore]
        public Unit? Unit
        {
            get => _unit.Value;
            set => UnitId = value?.Id;
        }
        readonly ForeignPropertyHelper<ProductComponent, Unit?> _unit;


        public static ProductComponent DesignModel => new()
        {
            Inn = Inn.DesignModel,
            Product = Product.DesignModel,
            Quantity = 100.0,
            Unit = Unit.DesignModel
        };
    }
}