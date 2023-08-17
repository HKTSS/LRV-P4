from shutil import copytree, ignore_patterns, rmtree
import os

def get_config():
    dict = {
        "base_train": None,
        "variant_list": [],
        "ignored_files": []
    }
    
    file = open(".releaseconfig", "r")
    category = None
    for line in file.read().splitlines():
        if len(line.strip()) == 0:
            continue
        if line.startswith("["):
            category = line[1:-1]
        else:
            if category == "base":
                dict["base_train"] = line
            if category == "variants":
                dict["variant_list"].append(line)
            if category == "ignore":
                dict["ignored_files"].append(line)

    file.close()
    return dict
    

release_path = "Release"
config = get_config()

if os.path.exists(release_path):
    rmtree(release_path)

os.mkdir(release_path);

# Copy base train
copytree(config["base_train"], os.path.join(release_path, config["base_train"]), ignore=ignore_patterns(*config["ignored_files"]))

for variant in config["variant_list"]:
    # Copy base first
    copytree(config["base_train"], os.path.join(release_path, variant), ignore=ignore_patterns(*config["ignored_files"]))
    
    # Apply patch
    copytree(variant, os.path.join(release_path, variant), ignore=ignore_patterns(*config["ignored_files"]), dirs_exist_ok=True)
