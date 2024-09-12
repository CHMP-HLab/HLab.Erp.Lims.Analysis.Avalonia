using System.Xml.Linq;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public static class Extensions
{
    public static XElement IgnoreNamespace(this XElement xElem)
    {
        XNamespace xmlns = "";
        var name = xmlns + xElem.Name.LocalName;
        return new XElement(name,
            from e in xElem.Elements()
            select e.IgnoreNamespace(),
            xElem.Attributes()
        );
    }
    public static XNode StripNamespaces(this XNode n)
    {
        var xe = n as XElement;
        if(xe == null)
            return n;
        var contents = 
            // add in all attributes there were on the original
            xe.Attributes()
                // eliminate the default namespace declaration
                .Where(xa => xa.Name.LocalName != "xmlns")
                .Cast<object>()
                // add in all other element children (nodes and elements, not just elements)
                .Concat(xe.Nodes().Select(node => node.StripNamespaces()).Cast<object>()).ToArray();
        var result = new XElement(XNamespace.None + xe.Name.LocalName, contents);
        return result;

    }

#if !LINQPAD
    public static T Dump<T>(this T t, string description = null)
    {
        if(description != null)
            Console.WriteLine(description);
        Console.WriteLine(t);
        return t;
    }
#endif
}