using LuccaDevises.dataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuccaDevises.businessLayer
{
    public class CurrencyConverter : ICurrencyConverter
    {

        private TreeNode root { get; set; }
        private List<Dictionary<int, double>> conversionCandidates { get; set; }

        public CurrencyConverter()
        {
            this.conversionCandidates = new List<Dictionary<int, double>>();
        }
        public int ConvertCurrency(BrainTeaser problem, List<ExchangeRate> rates)
        {
            root = new TreeNode(new ExchangeRate(problem.LeftCurrency, problem.LeftCurrency, 1.0));

            BuildTree(root, rates, problem.RightCurrency);


            return ConvertStepByStep(problem);
        }

        private int ConvertStepByStep(BrainTeaser problem)
        {
            var result = problem.Amount;
            if (conversionCandidates.Count < 1)
                return -1;
            var theGoodOne = conversionCandidates.OrderBy(cc => cc.Count).First();

            foreach (var step in theGoodOne.OrderBy(s => s.Key))
            {
                result = Math.Round(result * step.Value, 4, MidpointRounding.AwayFromZero);
            }
            result = Math.Round(result, MidpointRounding.AwayFromZero);
            return Convert.ToInt32(result);
        }



        private void BuildTree(TreeNode root, List<ExchangeRate> rates, string finalCurrency)
        {
            int level = 0;
            var nodes = root.GetNodesFromLevel(level);
            while (nodes.Count>0 && conversionCandidates.Count < 1) //stop tree building when  it is completed or if a candidate have been found - it is necessarily the shorter path 
            {
                foreach (var node in nodes)
                {
                    BuildNode(node, rates, finalCurrency);
                }

                ++level;
                nodes = root.GetNodesFromLevel(level);
            }
        }



        private void BuildNode(TreeNode parent, List<ExchangeRate> rates, string finalCurrency)
        {
            foreach (var rate in rates)
            {
                if (IsEligible(parent, rate))
                {
                    parent.AddChild(rate);

                    var uniqueBranchId = GetUniqueBranchId(parent, rate);
                    if (uniqueBranchId == finalCurrency)
                    {
                        conversionCandidates.Add(GetSteps(parent.Children.Last()));
                    }
                }
            }

        }



        private bool IsEligible(TreeNode parent, ExchangeRate candidate)
        {

            var uniqueBranchId = GetUniqueBranchId(parent, candidate);

            if (string.IsNullOrWhiteSpace(uniqueBranchId))
                return false;

            if (parent.GetBranchIds().Contains(uniqueBranchId))//avoid circular reference
                return false;

            if (parent.Children.Select(c => c.UniqueBranchId).Contains(uniqueBranchId))//avoid doublons in siblings
                return false;

            return true;
        }

        private string GetUniqueBranchId(TreeNode parent, ExchangeRate candidate)
        {
            if (candidate.LeftCurrency != parent.UniqueBranchId && candidate.RightCurrency != parent.UniqueBranchId)
                return "";

            string uniqueBranchId = candidate.LeftCurrency;
            if (candidate.LeftCurrency == parent.UniqueBranchId)
                uniqueBranchId = candidate.RightCurrency;
            return uniqueBranchId;
        }

        private Dictionary<int, double> GetSteps(TreeNode lastNode)
        {
            var steps = new Dictionary<int, double>();

            var stepRate = lastNode.Value.Rate;
            if (lastNode.Value.LeftCurrency == lastNode.UniqueBranchId)
                stepRate = lastNode.Value.InverseRate;

            steps.Add(lastNode.GetIndex(), stepRate);


            if (lastNode.IsRoot())
                return steps;
            else
                return steps.Concat(GetSteps(lastNode.Parent)).ToDictionary(pair => pair.Key, pair => pair.Value);
        }





    }
}
