let sassOptions;

sassOptions = {
    errLogToConsole: true,
    outputStyle: 'compressed',
    includePaths: [
        'node_modules/govuk-frontend/govuk',
        'Frontend/src/sass'
    ]
};

module.exports = sassOptions;