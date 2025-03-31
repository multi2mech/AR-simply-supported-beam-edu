import re

# Dictionary mapping Mathematica symbols to their Unicode equivalents
symbol_map = {
    # Lettere greche maiuscole
    r'\\\[Alpha\]': 'α',
    r'\\\[Beta\]': 'β',
    r'\\\[Gamma\]': 'γ',
    r'\\\[Delta\]': 'δ',
    r'\\\[Epsilon\]': 'ε',
    r'\\\[Zeta\]': 'ζ',
    r'\\\[Eta\]': 'η',
    r'\\\[Theta\]': 'θ',
    r'\\\[Iota\]': 'ι',
    r'\\\[Kappa\]': 'κ',
    r'\\\[Lambda\]': 'λ',
    r'\\\[Mu\]': 'μ',
    r'\\\[Nu\]': 'ν',
    r'\\\[Xi\]': 'ξ',
    r'\\\[Omicron\]': 'ο',
    r'\\\[Pi\]': 'π',
    r'\\\[Rho\]': 'ρ',
    r'\\\[Sigma\]': 'σ',
    r'\\\[Tau\]': 'τ',
    r'\\\[Upsilon\]': 'υ',
    r'\\\[Phi\]': 'φ',
    r'\\\[Chi\]': 'χ',
    r'\\\[Psi\]': 'ψ',
    r'\\\[Omega\]': 'ω',
    
    # Lettere greche minuscole
    r'\\\[CapitalAlpha\]': 'Α',
    r'\\\[CapitalBeta\]': 'Β',
    r'\\\[CapitalGamma\]': 'Γ',
    r'\\\[CapitalDelta\]': 'Δ',
    r'\\\[CapitalEpsilon\]': 'Ε',
    r'\\\[CapitalZeta\]': 'Ζ',
    r'\\\[CapitalEta\]': 'Η',
    r'\\\[CapitalTheta\]': 'Θ',
    r'\\\[CapitalIota\]': 'Ι',
    r'\\\[CapitalKappa\]': 'Κ',
    r'\\\[CapitalLambda\]': 'Λ',
    r'\\\[CapitalMu\]': 'Μ',
    r'\\\[CapitalNu\]': 'Ν',
    r'\\\[CapitalXi\]': 'Ξ',
    r'\\\[CapitalOmicron\]': 'Ο',
    r'\\\[CapitalPi\]': 'Π',
    r'\\\[CapitalRho\]': 'Ρ',
    r'\\\[CapitalSigma\]': 'Σ',
    r'\\\[CapitalTau\]': 'Τ',
    r'\\\[CapitalUpsilon\]': 'Υ',
    r'\\\[CapitalPhi\]': 'Φ',
    r'\\\[CapitalChi\]': 'Χ',
    r'\\\[CapitalPsi\]': 'Ψ',
    r'\\\[CapitalOmega\]': 'Ω',
    
    # Caratteri double-struck
    r'\\\[DoubleStruckCapitalA\]': '𝔸',
    r'\\\[DoubleStruckCapitalB\]': '𝔹',
    r'\\\[DoubleStruckCapitalC\]': 'ℂ',
    r'\\\[DoubleStruckCapitalD\]': '𝔻',
    r'\\\[DoubleStruckCapitalE\]': '𝔼',
    r'\\\[DoubleStruckCapitalF\]': '𝔽',
    r'\\\[DoubleStruckCapitalG\]': '𝔾',
    r'\\\[DoubleStruckCapitalH\]': 'ℍ',
    r'\\\[DoubleStruckCapitalI\]': '𝕀',
    r'\\\[DoubleStruckCapitalJ\]': '𝕁',
    r'\\\[DoubleStruckCapitalK\]': '𝕂',
    r'\\\[DoubleStruckCapitalL\]': '𝕃',
    r'\\\[DoubleStruckCapitalM\]': '𝕄',
    r'\\\[DoubleStruckCapitalN\]': 'ℕ',
    r'\\\[DoubleStruckCapitalO\]': '𝕆',
    r'\\\[DoubleStruckCapitalP\]': 'ℙ',
    r'\\\[DoubleStruckCapitalQ\]': 'ℚ',
    r'\\\[DoubleStruckCapitalR\]': 'ℝ',
    r'\\\[DoubleStruckCapitalS\]': '𝕊',
    r'\\\[DoubleStruckCapitalT\]': '𝕋',
    r'\\\[DoubleStruckCapitalU\]': '𝕌',
    r'\\\[DoubleStruckCapitalV\]': '𝕍',
    r'\\\[DoubleStruckCapitalW\]': '𝕎',
    r'\\\[DoubleStruckCapitalX\]': '𝕏',
    r'\\\[DoubleStruckCapitalY\]': '𝕐',
    r'\\\[DoubleStruckCapitalZ\]': 'ℤ',
    
    # Lettere double-struck minuscole
    r'\\\[DoubleStruckA\]': '𝕒',
    r'\\\[DoubleStruckB\]': '𝕓',
    r'\\\[DoubleStruckC\]': '𝕔',
    r'\\\[DoubleStruckD\]': '𝕕',
    r'\\\[DoubleStruckE\]': '𝕖',
    r'\\\[DoubleStruckF\]': '𝕗',
    r'\\\[DoubleStruckG\]': '𝕘',
    r'\\\[DoubleStruckH\]': '𝕙',
    r'\\\[DoubleStruckI\]': '𝕚',
    r'\\\[DoubleStruckJ\]': '𝕛',
    r'\\\[DoubleStruckK\]': '𝕜',
    r'\\\[DoubleStruckL\]': '𝕝',
    r'\\\[DoubleStruckM\]': '𝕞',
    r'\\\[DoubleStruckN\]': '𝕟',
    r'\\\[DoubleStruckO\]': '𝕠',
    r'\\\[DoubleStruckP\]': '𝕡',
    r'\\\[DoubleStruckQ\]': '𝕢',
    r'\\\[DoubleStruckR\]': '𝕣',
    r'\\\[DoubleStruckS\]': '𝕤',
    r'\\\[DoubleStruckT\]': '𝕥',
    r'\\\[DoubleStruckU\]': '𝕦',
    r'\\\[DoubleStruckV\]': '𝕧',
    r'\\\[DoubleStruckW\]': '𝕨',
    r'\\\[DoubleStruckX\]': '𝕩',
    r'\\\[DoubleStruckY\]': '𝕪',
    r'\\\[DoubleStruckZ\]': '𝕫',
    
    # Special double-struck symbols
    r'\\\[DoubleStruckZero\]': '𝟘',
    r'\\\[DoubleStruckOne\]': '𝟙',
    r'\\\[DoubleStruckTwo\]': '𝟚',
    r'\\\[DoubleStruckThree\]': '𝟛',
    r'\\\[DoubleStruckFour\]': '𝟜',
    r'\\\[DoubleStruckFive\]': '𝟝',
    r'\\\[DoubleStruckSix\]': '𝟞',
    r'\\\[DoubleStruckSeven\]': '𝟟',
    r'\\\[DoubleStruckEight\]': '𝟠',
    r'\\\[DoubleStruckNine\]': '𝟡',

    # Simboli speciali
    r'\\\[RightTee\]': '⊢',
    r'\\\[DoubleLeftTee\]': '⫤',
    r'\\\[DoubleRightTee\]': '⊨'
    # Add more symbols as needed
}

# Function to replace Mathematica symbols in code blocks
def replace_symbols(app, docname, source):
    result = source[0]
    for key, value in symbol_map.items():
        result = re.sub(key, value, result)
    source[0] = result

def setup(app):
    app.connect('source-read', replace_symbols)