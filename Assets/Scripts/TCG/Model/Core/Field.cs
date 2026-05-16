namespace TCG.Model.Core
{
    public class Field
    {
        public Lane[] Lanes;

        public Field()
        {
            Lanes = new Lane[3];
            for (int i = 0; i < 3; i++)
            {
                Lanes[i] = new Lane(i);
            }
        }
    }
}
