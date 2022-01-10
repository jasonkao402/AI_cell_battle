import numpy as np
import matplotlib.pyplot as plt
import glob
import random

for g in glob.glob("/unityProjects/AI_cell_battle/venv/output_*.txt"):
    print(g)
    with open(g) as cin:
        cg = f"#{random.randrange(0x1000000):06x}"
        for aline in cin.readlines():
            arr = list(map(int, aline.split()))
            x = np.arange(len(arr))
            plt.plot(x, arr, c=cg)
#plt.legend(['aaa'])
plt.show()