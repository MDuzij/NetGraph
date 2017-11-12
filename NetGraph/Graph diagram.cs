﻿using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Miscellaneous;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetGraph
{
    public partial class Graph_diagram : Form
    {
        public GViewer viewer { get; set; }
        public GraphGenerator graphGenerator { get; set; }

        public Graph_diagram(Graph graph)
        {
            InitializeComponent();

            viewer = new GViewer();
            viewer.EdgeInsertButtonVisible = false;
            viewer.MouseDoubleClick += new MouseEventHandler((sender, evt) =>
            {
                var viewerNode = (Node)viewer.SelectedObject;
                if (viewerNode != null)
                {
                    if (viewerNode.Edges.Count() > 0)
                        OpenChildGraphDiagram(viewerNode);
                    else
                    {
                        var detailedForm = new DetailedNodeInfo(viewerNode);
                        detailedForm.Show();
                    }
                }
            });

            ColorizeAndAjustFonts(graph);

            viewer.Graph = graph;
            viewer.Dock = DockStyle.Fill;
            var settings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings() { AdjustScale = true };
            LayoutHelpers.CalculateLayout(graph.GeometryGraph, settings, null);

            Controls.Add(viewer);

            ResumeLayout();
        }

        private static void ColorizeAndAjustFonts(Graph graph)
        {
            foreach (var item in graph.Nodes)
            {
                item.Label.FontSize = item.Edges.Count() / 3 < 5 ? 5 : item.Edges.Count() / 3;
                item.Attr.LabelMargin = 5;
            }
        }

        private void OpenChildGraphDiagram(Node node)
        {
            graphGenerator = new GraphGenerator();
            var childGraph = graphGenerator.GenerateChildGraph(node);
            Graph_diagram f = new Graph_diagram(childGraph);
            f.ShowDialog();
        }
    }
}
