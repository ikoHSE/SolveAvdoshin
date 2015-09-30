﻿using System;
using System.Collections.Generic;

namespace SolveAvdoshin
{
	public static class BooleanFunctions
	{
		static readonly BooleanOperation[][] AvdoshinBases = {
			new BooleanOperation[] {
				BooleanOperation.Zero,
				BooleanOperation.NOR,
				BooleanOperation.NotCoImp,
				BooleanOperation.NotA,
				BooleanOperation.NotImp,
				BooleanOperation.NotB,
				BooleanOperation.Xor,
				BooleanOperation.NAND,
				BooleanOperation.And,
				BooleanOperation.Eq,
				BooleanOperation.B,
				BooleanOperation.Imp,
				BooleanOperation.A,
				BooleanOperation.CoImp,
				BooleanOperation.Or,
				BooleanOperation.One
			},
			new BooleanOperation[] { BooleanOperation.NOR, },
			new BooleanOperation[] { BooleanOperation.NAND, },
			new BooleanOperation[] { BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.Imp, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Eq, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.Xor, BooleanOperation.And, },
		};

		static readonly BooleanVariable[][] AvdoshinVars = {
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
		};

		public static void PrintMinimaInAvdoshinBases(int n)
		{
			BooleanFunction f = new BooleanFunction(26);

			Console.WriteLine("\nМинимальные представления " + f.ToString("S") + " в базисах (для рисования в винлогике):");

			Console.WriteLine(" ☻ \tБазис\t\tВыражение");
			Console.WriteLine();

			for(int i = 0; i < AvdoshinBases.Length; i++) {
				Console.Write("{0,2}\t", i);

				if(i != 0) {
					foreach(var op in AvdoshinBases[i]) {
						Console.Write(BooleanExpression.PrintOperation(op) + " ");
					}
				}
				else {
					Console.Write("ОБЩИЙ");
				}

				Console.Write("\t\t");

				try {
					var ex = f.MininalExpressionInBasis(AvdoshinBases[i], AvdoshinVars[i]);

					Console.WriteLine(ex.ToString());
				}
				catch(TooLongToSearchForExpressionException) {
					Console.WriteLine("долго искать, пропускаем");
				}
			}
		}
	}

	class TooLongToSearchForExpressionException : Exception
	{

	}

	class BooleanFunction // TODO: Make private ?
	{
		protected int TruthTable;

		public BooleanFunction(int n)
		{
			TruthTable = n;
		}

		public bool Equals(BooleanFunction that)
		{
			return this.TruthTable == that.TruthTable;
		}

		public bool Eval(bool a, bool b, bool c)
		{
			return (TruthTable >> (7 - (a ? 4 : 0) - (b ? 2 : 0) - (c ? 1 : 0)) & 1) == 1;
		}
			
		public int Eval(int a, int b, int c)
		{
			return TruthTable >> (7 - a * 4 - b * 2 - c) & 1;
		}

		public static string VarRows()
		{
			string[] lines = new string[] { "A ", "B ", "C ", };

			for(int i = 0; i < 8; i++) {
				lines[0] += (i >> 2 & 1) + " ";
				lines[1] += (i >> 1 & 1) + " ";
				lines[2] += (i & 1) + " ";
			}

			return String.Join("\n", lines) + "\n";
		}

		public static string DelimRow()
		{
			string line = "--";

			for(int i = 0; i < 8; i++) {
				line += "--";
			}

			return line + "\n";
		}

		public string FuncRow()
		{
			string line = "F ";

			for(int i = 0; i < 8; i++) {
				line += (Eval((i >> 2 & 1) == 1, (i >> 1 & 1) == 1, (i & 1) == 1) ? 1 : 0) + " ";
			}

			return line + "\n";
		}

		public string ToString(string format)
		{
			if(format == "L") {
				string result = "";

				result += VarRows();
				result += DelimRow();
				result += FuncRow();

				return result;
			}
			else if(format == "S") {
				return "F_" + TruthTable;
			}
			else
				throw new ArgumentException("Invalid format string. Use L for long or S for short");
		}

		public override string ToString()
		{
			return ToString("S");
		}

		public BooleanExpression GetFDNF() // СДНФ
		{
			BooleanExpression ex = null, cons1;

			for(int i = 0; i < 6; i++) {
				if(!Eval((i >> 2 & 1) == 1, (i >> 1 & 1) == 1, (i & 1) == 1))
					continue;
					
				BooleanExpression varA, varB, varC;

				varA = new VarExpression(BooleanVariable.A, (i >> 2 & 1) != 1);
				varB = new VarExpression(BooleanVariable.B, (i >> 1 & 1) != 1);
				varC = new VarExpression(BooleanVariable.C, (i & 1) != 1);

				cons1 = new OpExpression(BooleanOperation.And,
					new OpExpression(BooleanOperation.And, varA, varB), varC);

				ex = ex != null ? new OpExpression(BooleanOperation.Or, cons1, ex) : cons1;
			}

			return ex;
		}

		public BooleanExpression MininalExpressionInBasis(BooleanOperation[] ops, BooleanVariable[] vars)
		{
			int depthLimit = ops.Length == 1 ? 10 : ops.Length == 2 ? 7 : 5;

			for(int i = 2; i <= depthLimit; i++) {
				foreach(var ex in BooleanExpression.AllExpressions(i, ops, vars)) {
					if(TruthTable == ex.GetTruthTable()) {
						return ex;
					}
				}
			}

			throw new TooLongToSearchForExpressionException();
		}
	}

	enum BooleanOperation { Zero, NOR, NotCoImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, CoImp, Or, One };
	enum BooleanVariable { A, B, C, Zero, One, };

	abstract class BooleanExpression
	{
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "</=", "(!A)", "=/>", "(!B)", "⨁", "|", "&", "==", "(B)", "=>", "(A)", "<=", "|", "1", };

		abstract public bool Eval(bool a, bool b, bool c);
		abstract new public string ToString();
		abstract public int CountVariables();
		abstract public void SetIthVar(int i, BooleanVariable value);
		abstract public int setIthOp(int i, BooleanOperation value);

		public int Eval(int a, int b, int c)
		{
			return Eval(a == 1, b == 1, c == 1) ? 1 : 0;
		}

		public int GetTruthTable()
		{
			int truthTable = 0;

			for(int i = 0; i < 8; i++) {
				truthTable *= 2;
				truthTable += Eval((i >> 2) & 1, (i >> 1) & 1, i & 1);
			}

			return truthTable;
		}

		public static IEnumerable<BooleanExpression> AllTrees(int size)
		{
			if(size == 0) {
				yield return new VarExpression(BooleanVariable.A);
			}
			else {
				for(int i = 0; i < size; i++) {
					foreach(var l in AllTrees(i)) {
						foreach(var r in AllTrees(size - 1 - i)) {
							yield return new OpExpression(BooleanOperation.And, l, r);
						}
					}
				}
			}
		}

		public static readonly BooleanVariable[] AllVars = { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, };

		public static bool CheckVarList(BooleanVariable[] varList)
		{
			foreach(var variable in BooleanExpression.AllVars) {
				bool varPresent = false;

				foreach(var v in varList) {
					if(v == variable) {
						varPresent = true;
						break;
					}
				}

				if(!varPresent) return false;
			}

			return true;
		}

		public static IEnumerable<BooleanExpression> AllExpressions(int size, BooleanOperation[] ops,
			BooleanVariable[] vars)
		{
			foreach(var ex in AllTrees(size)) {
				foreach(var varList in Combinatorics.AllNTuples(vars, size + 1)) {
//					if(!CheckVarList(varList, vars))
//						continue;
					
					foreach(var opList in Combinatorics.AllNTuples(ops, size)) {
						for(int i = 0; i < size; i++) {
							ex.setIthOp(i, opList[i]);
						}

						for(int i = 0; i < size + 1; i++) {
							ex.SetIthVar(i, varList[i]);
						}

						yield return ex;
					}
				}
			}
		}

		public static string PrintOperation(BooleanOperation op)
		{
			return OpSymbols[(int)op];
		}
	}

	class OpExpression : BooleanExpression
	{
		BooleanOperation Op;
		BooleanExpression Left, Right;

		public OpExpression(BooleanOperation op, BooleanExpression left, BooleanExpression right)
		{
			Op = op;
			Left = left;
			Right = right;
		}

		public OpExpression(BooleanOperation op, BooleanVariable left, BooleanExpression right)
		{
			Op = op;
			Left = new VarExpression(left);
			Right = right;
		}

		public OpExpression(BooleanOperation op, BooleanExpression left, BooleanVariable right)
		{
			Op = op;
			Left = left;
			Right = new VarExpression(right);
		}

		public OpExpression(BooleanOperation op, BooleanVariable left, BooleanVariable right)
		{
			Op = op;
			Left = new VarExpression(left);
			Right = new VarExpression(right);
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			bool aVal = Left.Eval(a, b, c);
			bool bVal = Right.Eval(a, b, c);

			switch(Op) {
			case BooleanOperation.Zero:
				return false;
			case BooleanOperation.NOR:
				return !(aVal || bVal);
			case BooleanOperation.NotCoImp:
				return !aVal && bVal;
			case BooleanOperation.NotA:
				return !aVal;
			case BooleanOperation.NotImp:
				return aVal && !bVal;
			case BooleanOperation.NotB:
				return !bVal;
			case BooleanOperation.Xor:
				return aVal != bVal;
			case BooleanOperation.NAND:
				return !(aVal && bVal);
			case BooleanOperation.And:
				return aVal && bVal;
			case BooleanOperation.Eq:
				return aVal == bVal;
			case BooleanOperation.B:
				return bVal;
			case BooleanOperation.Imp:
				return !aVal || bVal;
			case BooleanOperation.A:
				return aVal;
			case BooleanOperation.CoImp:
				return aVal || !bVal;
			case BooleanOperation.Or:
				return aVal || bVal;
			case BooleanOperation.One:
				return true;
			default:
				throw new ArgumentException();
			}
		}

		public override string ToString()
		{
			switch(Op) {
			case BooleanOperation.A:
				return Left.ToString();
			case BooleanOperation.B:
				return Right.ToString();
			case BooleanOperation.One:
			case BooleanOperation.Zero:
				return PrintOperation(Op);
			case BooleanOperation.NotA:
				return "!" + Left.ToString();
			case BooleanOperation.NotB:
				return "!" + Right.ToString();
			default:
				return "(" + Left.ToString() + " " + PrintOperation(Op) + " " + Right.ToString() + ")";
			}
		}

		public override int CountVariables()
		{
			int res = 0;

			res += Left.CountVariables();
			res += Right.CountVariables();

			return res;
		}

		public override void SetIthVar(int i, BooleanVariable value)
		{
			if(i >= Left.CountVariables()) {
				Right.SetIthVar(i - Left.CountVariables(), value);
			}
			else {
				Left.SetIthVar(i, value);
			}
		}

		public override int setIthOp(int i, BooleanOperation value)
		{
			if(i < 0) {
				return 0;
			}
			else {
				if(i == 0) {
					Op = value;
				}

				int res = 1;

				res += Left.setIthOp(i - res, value);
				res += Right.setIthOp(i - res, value);

				return res;
			}
		}
	}

	class VarExpression : BooleanExpression
	{
		static readonly string[] VarSymbols = new string[] { "A", "B", "C", "0", "1", };

		BooleanVariable Var;

		public VarExpression(BooleanVariable var, bool not = false)
		{
			Var = var;
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			switch(Var) {
			case BooleanVariable.A:
				return a;
			case BooleanVariable.B:
				return b;
			case BooleanVariable.C:
				return c;
			case BooleanVariable.Zero:
				return false;
			case BooleanVariable.One:
				return true;
			default:
				throw new Exception("Nigga wat?");
			}
		}

		public override string ToString()
		{
			return VarSymbols[(int)Var];
		}

		public override int CountVariables()
		{
			return 1;
		}

		public override void SetIthVar(int i, BooleanVariable variable)
		{
			this.Var = variable;
		}

		public override int setIthOp(int i, BooleanOperation value)
		{
			return 0;
		}
	}
}

