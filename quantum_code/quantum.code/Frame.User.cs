using System;

namespace Quantum {
    unsafe partial class Frame
    {
        public byte[] Grid => _grid;
        private byte[] _grid;

        partial void InitUser()
        {
            _grid = new byte[RuntimeConfig.GridSize];
        }

        partial void SerializeUser(FrameSerializer serializer)
        {
            serializer.Stream.SerializeArrayLength<Byte>(ref _grid);
            for (int i = 0; i < Grid.Length; i++)
            {
                serializer.Stream.Serialize(ref Grid[i]);
            }
        }

        partial void CopyFromUser(Frame frame)
        {
            Array.Copy(frame._grid, _grid, frame._grid.Length);
        }
    }
}
