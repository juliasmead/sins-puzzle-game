#!/usr/bin/env python
 
from gimpfu import *
import os
 
def add_border(image, layer, shrink, grow, feather):
  pdb.gimp_by_color_select(layer,'white',255,0,TRUE,0,0,0)
  pdb.gimp_selection_shrink(image,shrink)
  pdb.plug_in_sel2path(image,layer)
  pdb.gimp_by_color_select(layer,'white',255,0,TRUE,0,0,0)
  curpath = pdb.gimp_path_get_current(image)
  pdb.gimp_path_to_selection(image,curpath,1,TRUE,FALSE,0,0)
  pdb.gimp_path_delete(image,curpath)
  pdb.gimp_selection_grow(image,grow)
  pdb.gimp_selection_feather(image,feather)
  pdb.gimp_bucket_fill(layer,0,0,100,0,0,0,0)
  pdb.gimp_selection_clear(image)
 
register(
  proc_name="add_border",
  blurb="Adds a border to the current image",
  help="Adds a border to the current image with the given params",
  author="Alex Zilbersher",
  copyright="Alex Zilbersher",
  date="2018",
  label="<Image>/Filters/Border/Add Border",
  imagetypes="*",
  params=[
  (PF_SPINNER, "shrink", "Indent", 4, (1, 3000, 1)),
  (PF_SPINNER, "grow", "Width", 2, (1, 3000, 1)),
  (PF_SPINNER, "feather", "Feather", 5, (1, 3000, 1)),
  ],
  results=[],
  function=add_border)
 
main()
