/// <binding BeforeBuild='dev' />
var gulp = require('gulp');

//require('./gulp/tasks/dev');
require('./gulp/tasks/default');

gulp.task('default', gulp.series('govuk-js', 'copy-js', 'copy-provider-search-js', 'copy-registration-upload-js', 'copy-assessment-upload-js', 'copy-result-upload-js', 'copy-english-maths-question-js', 'copy-user-guide-js', 'copy-cookies-js', 'copy-registration-specialism-question-js', 'copy-session-timeout-js', 'copy-assets', 'merge-css',
    (done) => {
        done();
    }));

gulp.task('dev', gulp.series('govuk-js', 'copy-js', 'copy-provider-search-js', 'copy-registration-upload-js', 'copy-assessment-upload-js', 'copy-result-upload-js', 'copy-english-maths-question-js', 'copy-user-guide-js', 'copy-cookies-js', 'copy-registration-specialism-question-js', 'copy-session-timeout-js', 'copy-assets', 'merge-css',
    (done) => {
        done();
    }));