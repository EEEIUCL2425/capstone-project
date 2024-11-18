import EnvAPI
import UnityEngine
import random


Objects = ["Sphere"]
Instances = [15]

EnvAPI.destroy_all_objects()
spawned_objects = EnvAPI.spawn_objects(random.uniform(0,100), Objects, Instances, 0)

