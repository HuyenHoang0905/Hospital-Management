using System.Drawing;

namespace DevComponents.Instrumentation
{
    public class Seg7Element : DigitalElement
    {
        public Seg7Element(NumericIndicator numIndicator)
            : base(numIndicator, 7, 6)
        {
        }

        #region RecalcSegments

        public override void RecalcSegments()
        {
            CalcSegment0();
            CalcSegment1();
            CalcSegment2();
            CalcSegment3();
            CalcSegment4();
            CalcSegment5();
            CalcSegment6();
        }

        #region CalcSegment0

        private void CalcSegment0()
        {
            Point[] pts = SegPoints[0];

            pts[0].X = SegWidth + 1; pts[0].Y = 0;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = 0;
            pts[2].X = ElemWidth - SegWidthHalf - 1; pts[2].Y = SegWidthHalf;
            pts[3].X = pts[1].X; pts[3].Y = SegWidth;
            pts[4].X = pts[0].X; pts[4].Y = SegWidth;
            pts[5].X = SegWidthHalf + 1; pts[5].Y = pts[2].Y;
        }

        #endregion

        #region CalcSegment1

        private void CalcSegment1()
        {
            Point[] pts = SegPoints[1];

            pts[0].X = 0; pts[0].Y = SegWidth + 1;
            pts[1].X = SegWidthHalf; pts[1].Y = SegWidthHalf + 1;
            pts[2].X = SegWidth; pts[2].Y = SegWidth + 1;
            pts[3].X = SegWidth; pts[3].Y = ElemHeightHalf - SegWidthHalf - 1;
            pts[4].X = 4; pts[4].Y = ElemHeightHalf - 1;
            pts[5].X = 0; pts[5].Y = ElemHeightHalf - 1;
        }

        #endregion

        #region CalcSegment2

        private void CalcSegment2()
        {
            Point[] pts = SegPoints[2];

            pts[0].X = ElemWidth - SegWidth; pts[0].Y = SegWidth + 1;
            pts[1].X = ElemWidth - SegWidthHalf; pts[1].Y = SegWidthHalf + 1;
            pts[2].X = ElemWidth; pts[2].Y = SegWidth + 1;
            pts[3].X = ElemWidth; pts[3].Y = ElemHeightHalf - 1;
            pts[4].X = ElemWidth - 4; pts[4].Y = ElemHeightHalf - 1;
            pts[5].X = ElemWidth - SegWidth; pts[5].Y = ElemHeightHalf - SegWidthHalf - 1;
        }

        #endregion

        #region CalcSegment3

        private void CalcSegment3()
        {
            Point[] pts = SegPoints[3];

            pts[0].X = SegWidth + 1; pts[0].Y = ElemHeightHalf - SegWidthHalf;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = ElemHeightHalf - SegWidthHalf;
            pts[2].X = ElemWidth - 5; pts[2].Y = ElemHeightHalf;
            pts[3].X = ElemWidth - SegWidth - 1; pts[3].Y = ElemHeightHalf + SegWidthHalf;
            pts[4].X = SegWidth + 1; pts[4].Y = ElemHeightHalf + SegWidthHalf;
            pts[5].X = 5; pts[5].Y = ElemHeightHalf;
        }

        #endregion

        #region CalcSegment4

        private void CalcSegment4()
        {
            Point[] pts = SegPoints[4];

            pts[0].X = 0; pts[0].Y = ElemHeightHalf + 1;
            pts[1].X = 4; pts[1].Y = ElemHeightHalf + 1;
            pts[2].X = SegWidth; pts[2].Y = ElemHeightHalf + SegWidthHalf + 1;
            pts[3].X = SegWidth; pts[3].Y = ElemHeight - SegWidth - 1;
            pts[4].X = SegWidthHalf; pts[4].Y = ElemHeight - SegWidthHalf - 1;
            pts[5].X = 0; pts[5].Y = ElemHeight - SegWidth - 1;
        }

        #endregion

        #region CalcSegment5

        private void CalcSegment5()
        {
            Point[] pts = SegPoints[5];

            pts[0].X = ElemWidth - SegWidth; pts[0].Y = ElemHeightHalf + SegWidthHalf + 1;
            pts[1].X = ElemWidth - 4; pts[1].Y = ElemHeightHalf + 1;
            pts[2].X = ElemWidth; pts[2].Y = ElemHeightHalf + 1;
            pts[3].X = ElemWidth; pts[3].Y = ElemHeight - SegWidth - 1;
            pts[4].X = ElemWidth - SegWidthHalf; pts[4].Y = ElemHeight - SegWidthHalf - 1;
            pts[5].X = ElemWidth - SegWidth; pts[5].Y = ElemHeight - SegWidth - 1;
        }

        #endregion

        #region CalcSegment6

        private void CalcSegment6()
        {
            Point[] pts = SegPoints[6];

            pts[0].X = SegWidth + 1; pts[0].Y = ElemHeight - SegWidth;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = ElemHeight - SegWidth;
            pts[2].X = ElemWidth - SegWidthHalf - 1; pts[2].Y = ElemHeight - SegWidthHalf;
            pts[3].X = ElemWidth - SegWidth - 1; pts[3].Y = ElemHeight;
            pts[4].X = SegWidth + 1; pts[4].Y = ElemHeight;
            pts[5].X = SegWidthHalf + 1; pts[5].Y = ElemHeight - SegWidthHalf;
        }

        #endregion

        #endregion

        #region GetCharSegments

        public override int GetDigitSegments(char value)
        {
            switch (value)
            {
                case '0': return ((int)Seg7Segments.Zero);
                case '1': return ((int)Seg7Segments.One);
                case '2': return ((int)Seg7Segments.Two);
                case '3': return ((int)Seg7Segments.Three);
                case '4': return ((int)Seg7Segments.Four);
                case '5': return ((int)Seg7Segments.Five);
                case '6': return ((int)Seg7Segments.Six);
                case '7': return ((int)Seg7Segments.Seven);
                case '8': return ((int)Seg7Segments.Eight);
                case '9': return ((int)Seg7Segments.Nine);

                case 'a': return ((int)Seg7Segments.a);
                case 'b': return ((int)Seg7Segments.b);
                case 'c': return ((int)Seg7Segments.c);
                case 'd': return ((int)Seg7Segments.d);
                case 'e': return ((int)Seg7Segments.e);
                case 'f': return ((int)Seg7Segments.f);
                case 'g': return ((int)Seg7Segments.g);
                case 'h': return ((int)Seg7Segments.h);
                case 'i': return ((int)Seg7Segments.i);
                case 'j': return ((int)Seg7Segments.j);
                case 'k': return ((int)Seg7Segments.k);
                case 'l': return ((int)Seg7Segments.l);
                case 'm': return ((int)Seg7Segments.m);
                case 'n': return ((int)Seg7Segments.n);
                case 'o': return ((int)Seg7Segments.o);
                case 'p': return ((int)Seg7Segments.p);
                case 'q': return ((int)Seg7Segments.q);
                case 'r': return ((int)Seg7Segments.r);
                case 's': return ((int)Seg7Segments.s);
                case 't': return ((int)Seg7Segments.t);
                case 'u': return ((int)Seg7Segments.u);
                case 'v': return ((int)Seg7Segments.v);
                case 'w': return ((int)Seg7Segments.w);
                case 'x': return ((int)Seg7Segments.x);
                case 'y': return ((int)Seg7Segments.y);
                case 'z': return ((int)Seg7Segments.z);

                case 'A': return ((int)Seg7Segments.A);
                case 'B': return ((int)Seg7Segments.B);
                case 'C': return ((int)Seg7Segments.C);
                case 'D': return ((int)Seg7Segments.D);
                case 'E': return ((int)Seg7Segments.E);
                case 'F': return ((int)Seg7Segments.F);
                case 'G': return ((int)Seg7Segments.G);
                case 'H': return ((int)Seg7Segments.H);
                case 'I': return ((int)Seg7Segments.I);
                case 'J': return ((int)Seg7Segments.J);
                case 'K': return ((int)Seg7Segments.K);
                case 'L': return ((int)Seg7Segments.L);
                case 'M': return ((int)Seg7Segments.M);
                case 'N': return ((int)Seg7Segments.N);
                case 'O': return ((int)Seg7Segments.O);
                case 'P': return ((int)Seg7Segments.P);
                case 'Q': return ((int)Seg7Segments.Q);
                case 'R': return ((int)Seg7Segments.R);
                case 'S': return ((int)Seg7Segments.S);
                case 'T': return ((int)Seg7Segments.T);
                case 'U': return ((int)Seg7Segments.U);
                case 'V': return ((int)Seg7Segments.V);
                case 'W': return ((int)Seg7Segments.W);
                case 'X': return ((int)Seg7Segments.X);
                case 'Y': return ((int)Seg7Segments.Y);
                case 'Z': return ((int)Seg7Segments.Z);

                case ' ': return ((int)Seg7Segments.Space);
                case '-': return ((int)Seg7Segments.Dash);
                case '=': return ((int)Seg7Segments.Equals);
                case '(': return ((int)Seg7Segments.LParen);
                case '}': return ((int)Seg7Segments.RParen);
                case '[': return ((int)Seg7Segments.LBracket);
                case ']': return ((int)Seg7Segments.RBracket);
                case '_': return ((int)Seg7Segments.Underline);

                // Add callout code to get pattern

                default: return ((int)Seg7Segments.None);
            }
        }

        #endregion
    }

    #region Enums

    #region Seg7Segments

    // ReSharper disable InconsistentNaming

    public enum Seg7Segments : int
    {
        None = 0x0,

        Zero = 0x77,
        One = 0x24,
        Two = 0x5D,
        Three = 0x6D,
        Four = 0x2E,
        Five = 0x6B,
        Six = 0x7B,
        Seven = 0x25,
        Eight = 0x7F,
        Nine = 0x6F,

        a = 0x7D,
        b = 0x7A,
        c = 0x58,
        d = 0x7C,
        e = 0x5F,
        f = 0x1B,
        g = 0x6F,
        h = 0x3A,
        i = 0x51,
        j = 0x61,
        k = 0x3E,
        l = 0x50,
        m = 0x31,
        n = 0x38,
        o = 0x78,
        p = 0x1F,
        q = 0x2F,
        r = 0x18,
        s = 0x6B,
        t = 0x5A,
        u = 0x70,
        v = 0x70,
        w = 0x46,
        x = 0x3E,
        y = 0x6E,
        z = 0x5D,

        A = 0x3F,
        B = 0x7F,
        C = 0x53,
        D = 0x77,
        E = 0x5B,
        F = 0x1B,
        G = 0x73,
        H = 0x3E,
        I = 0x24,
        J = 0x74,
        K = 0x3E,
        L = 0x52,
        M = 0x31,
        N = 0x38,
        O = 0x77,
        P = 0x1F,
        Q = 0x2F,
        R = 0x13,
        S = 0x6B,
        T = 0x13,
        U = 0x76,
        V = 0x76,
        W = 0x46,
        X = 0x3E,
        Y = 0x6E,
        Z = 0x5D,

        Space = 0x00,
        Dash = 0x08,
        Equals = 0x48,
        LParen = 0x63,
        RParen = 0x65,
        LBracket = 0x63,
        RBracket = 0x65,
        Underline = 0x40,
    }

    // ReSharper restore InconsistentNaming

    #endregion

    #endregion
}
