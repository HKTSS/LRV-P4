import glob
from PIL import Image
import math

# root_dir needs a trailing slash (i.e. /root/dir/)

dimlist = {}

def uppotwo(x):
    return pow(2, math.ceil(math.log(x)/math.log(2)))

for filename in glob.iglob('**/*.png', recursive=True):
    if "train.png" in filename or "Temp_ReplicaSticker" in filename or "/Night/" in filename or "/NoLight/" in filename:
        continue
    with Image.open(filename) as image:
         w, h = image.size
         # https://github.com/leezer3/OpenBVE/issues/226
         wW = uppotwo(w)
         hH = uppotwo(h)
         vram = (wW * 4) * (hH)
         dimlist[filename] = vram

total = 0
for path in sorted(dimlist, key=dimlist.get):
    vram = dimlist[path]
    total += vram
    print(str(vram / 1048576) + "MB" + ": " + path)

print("Total: " + str(total / 1048576) + "MB")
