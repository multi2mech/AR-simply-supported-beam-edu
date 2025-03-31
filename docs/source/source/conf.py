# Configuration file for the Sphinx documentation builder.
#
# For the full list of built-in configuration values, see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Project information -----------------------------------------------------
# https://www.sphinx-doc.org/en/master/usage/configuration.html#project-information

project = 'AR for beams'
copyright = '2025, Alessandro Mastrofini'
author = 'Alessandro Mastrofini'
release = '2.0'

import os
import sys
# sys.path.insert(0, os.path.abspath('../source/py/myAceplaque'))
# sys.path.insert(0, os.path.abspath('../source/py/myModulesV2'))
# sys.path.insert(0, os.path.abspath('../source/py/mesh-importer_v2'))
# sys.path.insert(0, os.path.abspath('../source/py/myPatientsModulesV2'))

# -- General configuration ---------------------------------------------------
# https://www.sphinx-doc.org/en/master/usage/configuration.html#general-configuration
sys.path.insert(0, os.path.abspath('../source/_ext'))

extensions = [
    'myst_nb',
    'sphinx.ext.mathjax',  # Optional, for rendering LaTeX equations
    'sphinx.ext.autodoc',  # Optional, if you are documenting code
    'sphinx.ext.viewcode', # Optional, to add links to highlighted source code
    'sphinx.ext.mathjax',  # Usa MathJax per equazioni in HTML
    'sphinx.ext.imgmath',  # Genera immagini delle equazioni
    'rst2pdf.pdfbuilder',
    'mathematica_symbols',
    'sphinxcontrib.bibtex'
]

nb_execution_mode = 'off'  # or 'force', 'off', 'cache'

templates_path = ['_templates']
exclude_patterns = []

bibtex_bibfiles = ['references.bib'] 

latex_elements = {
    'preamble': r'''
    \usepackage{amsmath}
    \usepackage{amsfonts}
    \newcommand{\vect}[1]{\mathbf{#1}}
    '''
}

latex_engine = 'pdflatex'  # o 'xelatex' o 'lualatex'

# -- Options for HTML output -------------------------------------------------
# https://www.sphinx-doc.org/en/master/usage/configuration.html#options-for-html-output

# html_theme = 'sphinx_rtd_theme'
# html_static_path = ['_static']

html_static_path = ['_static']
html_js_files = []
html_css_files = []

html_extra_path = []

html_theme_options = {
    # your theme options here (optional)
}

html_use_index = True
html_copy_source = True

# This is the key:
html_theme = 'sphinx_rtd_theme'  # or your theme
html_theme_options = {}
html_show_sourcelink = True
html_show_sphinx = False

nb_mime_priority_overrides = [('pdf', 'text/plain', 100)]
