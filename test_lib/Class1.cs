namespace test_lib
{
    public class Class1
    {
        public Class1() : this (1, 2) { }

        public Class1(int a) : this(a, 2) { }

        public Class1(int a, int b)
        {
            memb_1 = a;
            memb_2 = b;
        }

        public void GetMembers(out int a, out int b)
        {
            a = memb_1;
            b = memb_2;
        }

        private int memb_1;
        private int memb_2;
    }
}
