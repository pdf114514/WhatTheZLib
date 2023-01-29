# This is working correctly / expectedly
import zlib

with open("Data.Sav", "rb") as f:
    header = f.read(0x10)
    original = f.read()

decompressed = zlib.decompress(original)
with open("python_decompressed", "wb") as f:
    f.write(decompressed)
    print(f"Wrote {len(decompressed)} bytes to python_decompressed")

compressed = zlib.compress(decompressed, 6)
with open("python_compressed", "wb") as f:
    f.write(compressed)
    print(f"Wrote {len(compressed)} bytes to python_compressed")

with open("python_result", "wb") as f:
    f.write(header)
    f.write(compressed)
    print(f"Wrote {len(header) + len(compressed)} bytes to python_result")