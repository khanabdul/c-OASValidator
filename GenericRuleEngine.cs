using System;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Helpers;
using System.Collections.Generic;
using System.Linq;

class GenericRuleEngine
{
    private static List<YamlNode> GetValues(List<string> nodes, YamlNode node)
    {
        List<YamlNode> nodeToReturn = new List<YamlNode>();
        var nodeslist = new List<string>(nodes);
        foreach (string nodeitem in nodes)
        {
            //Console.WriteLine("\n"+node.NodeType);
            //Console.WriteLine("\n" + nodeitem);
            
            if (node.NodeType.ToString() == "Sequence")
            {
                // ((YamlDotNet.RepresentationModel.YamlSequenceNode)node).Children
                foreach (var item in ((YamlSequenceNode)node).Children)
                {
                    nodeToReturn.AddRange(GetValues(nodeslist, item));
                }

            }
            else if (node.NodeType.ToString() == "Mapping" && ((YamlMappingNode)node).Children.ContainsKey(nodeitem))
            {
                node = node[(nodeitem)];
            }
            if (node.NodeType.ToString() == "Scalar")
            {
                nodeToReturn.Add(node);
            }
            if (node == null)
            {
                return null;
            }

        }
        return nodeToReturn;
    }
    public static List<Tuple<bool, int, int>> ValidateRule(YamlNode rootNode, string tagName, string expectedType, string regex)
    {
        List<Tuple<bool, int, int>> results = new List<Tuple<bool, int, int>>();
        //var node = rootNode.Children[new YamlScalarNode(tagName)];
        var nodes = tagName.Split(".");
        //var node = rootNode;
        //var seqNode = new YamlDotNet.RepresentationModel.YamlSequenceNode().Children.Count;
        //var mappingNode1 = new YamlDotNet.RepresentationModel.YamlMappingNode().Children.Count;
        //var tscalerNode1 = new YamlDotNet.RepresentationModel.YamlScalarNode().Value;
        List<string> listNodes = new List<string>(nodes);
        List<YamlNode> resultvalues = GetValues(listNodes, rootNode);
       // var value = node.ToString();
        //var node = rootNode.AllNodes.[(tagName)];
        //Console.WriteLine(value);
        //var node = rootNode.Children[new YamlScalarNode(tagName)];
        foreach (var node in resultvalues.Distinct())
        {
            //if (node.Tag  == tagName)
            {
                var value = node.ToString();
                Console.WriteLine("values: "+value);
                if (node != null)
                {
                    //var value = node.ToString();
                    if (expectedType == "int")
                    {
                        int result;
                        if (!int.TryParse(value, out result))
                        {
                            results.Add(Tuple.Create(false, node.Start.Line, node.End.Line));
                        }
                        else
                        {
                            results.Add(Tuple.Create(true, node.Start.Line, node.End.Line));
                        }
                    }
                    else if (expectedType == "string")
                    {
                        if (!string.IsNullOrEmpty(regex))
                        {
                            if (!Regex.IsMatch(value, regex))
                            {
                                results.Add(Tuple.Create(false, node.Start.Line, node.End.Line));
                            }
                            else
                            {
                                results.Add(Tuple.Create(true, node.Start.Line, node.End.Line));
                            }
                        }
                        else
                        {
                            results.Add(Tuple.Create(true, node.Start.Line, node.End.Line));
                        }
                    }
                }
                else
                {
                     results.Add(Tuple.Create(false, -1, -1));
                }
            }

        }
        return results;//Tuple.Create(true, 0, 0);
    }

    
}