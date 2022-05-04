#!/usr/bin/env python3
import subprocess
import xml.etree.ElementTree as ET
import csv
ET.register_namespace('', 'urn:strusoft')

# ----INPUT----
LoadStep = 5 # total load steps
ConvCrit = 0.02 # convergence criteria
model_path = 'C:\\temp\\data\\Model.struxml'
program_path = 'C:\\program files\\strusoft\\fem-design 18\\fd3dstruct'
script_path = 'C:\\temp\\data\\run.fdscript'
reaction_result_path = 'C:\\temp\\data\\reac_result.csv'
# -------------

# THIS PART WILL FIND THE ORIGINAL LOAD COMBINATION GAMMA FACTORS AND SAVE THEM
tree_org = ET.parse(model_path)
root_org = tree_org.getroot()
gamma_org = []
load_comb_sum_org = []
for load_comb_org in root_org[0].iter('{urn:strusoft}load_combination'):
    load_comb_sum_org.append(load_comb_org)
for i in range(len(load_comb_sum_org[0])):
    gamma_org.append(load_comb_sum_org[0][i].attrib['gamma'])
# -----------------------------------------------------------------------------

for step in range(1, LoadStep+1): # Each iteration is one load step
    print('Current load step: ' + str(step / LoadStep * 100) + '% of total load')
    tree = ET.parse(model_path)
    root = tree.getroot()

    # --- Set load step by multiplying the load combination factors with "step" ---
    load_comb_sum = []
    for load_comb in root[0].iter('{urn:strusoft}load_combination'):
        load_comb_sum.append(load_comb)
    for i in range(len(load_comb_sum[0])):
        load_comb_sum[0][i].attrib['gamma'] = str(step * float(gamma_org[i]) / LoadStep)
    tree.write(model_path)
    conv = []
    check = 1
    k = 0
    while check > ConvCrit: # While loop running until result has converged
        subprocess.run([program_path, '/s', script_path])
        tree = ET.parse(model_path)
        root = tree.getroot()
        int_reac = []
        with open(reaction_result_path) as csvimport: # Read the CSV-file
            reader = csv.reader(csvimport, delimiter='\t')
            for row in reader:
                if len(row) == 0:
                    break
                int_reac.append(float(row[7]))
        print(['Reactions for current iteration:', int_reac])
        support_sum = []
        stifflist = []
        diff = []
        for support in root[0].iter('{urn:strusoft}motions'): # Find all supports
            support_sum.append(support)
        for u in range(len(support_sum)): # Change stiffness of supports
            stiffness = 1000 + 500000/abs(int_reac[u])
            support_sum[u].attrib["z_neg"] = str(stiffness)
            stifflist.append(stiffness)

        # --- Calculate difference between the stiffness from previous iteration and this iteration to check convergence ---
        conv.append(stifflist)
        for j in range(len(stifflist)):
            if k == 0:
                break
            diff.append(abs((conv[k][j]-conv[k-1][j])/conv[k-1][j]))
        if k != 0:
            check = max(diff)
            print('Difference in stiffness from previous iteration: ' + str(check))
        k = k + 1
        tree.write(model_path)