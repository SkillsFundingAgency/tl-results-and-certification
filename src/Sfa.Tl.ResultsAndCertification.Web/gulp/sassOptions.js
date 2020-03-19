let sassOptions;

sassOptions = {
    errLogToConsole: true,
    outputStyle: 'compressed',
    includePaths: [
        'node_modules/govuk-frontend/govuk',
        'node_modules/accessible-autocomplete',
        'Frontend/src/sass'
    ]
};

module.exports = sassOptions;