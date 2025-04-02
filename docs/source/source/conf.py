# Configuration file for the Sphinx documentation builder.
#
# For the full list of built-in configuration values, see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Project information -----------------------------------------------------
# https://www.sphinx-doc.org/en/master/usage/configuration.html#project-information

project = 'AR for beams'
copyright = '2025 - multi2mech.com - University of Rome Tor Vergata'
author = 'Alessandro Mastrofini, Michele Marino'
release = '0.1.0'

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
    'sphinxcontrib.bibtex',
    'sphinx_copybutton'
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
templates_path = ['_templates']
html_js_files = []
html_css_files = []

html_extra_path = []

html_theme_options = {
    # your theme options here (optional)
}

# Set the output to use relative paths
html_theme = 'sphinx_rtd_theme'  # or your theme
html_baseurl = ''
html_use_opensearch = False
html_copy_source = False
html_show_sphinx = False

# 👇 THIS is the magic line:
html_relative_urls = True  # THIS forces relative URLs for static assets

nb_mime_priority_overrides = [('pdf', 'text/plain', 100)]
