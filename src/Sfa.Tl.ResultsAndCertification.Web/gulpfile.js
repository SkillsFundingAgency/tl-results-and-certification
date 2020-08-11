/// <binding BeforeBuild='dev' />


var gulp = require('gulp');

//require('./gulp/tasks/dev');
require('./gulp/tasks/default');

gulp.task('default', gulp.series('govuk-js', 'copy-js', 'copy-provider-search-js', 'copy-registration-upload-js', 'copy-user-guide-js', 'copy-assets', 'merge-css',
    (done) => {
        done();
    }));

gulp.task('dev', gulp.series('govuk-js', 'copy-js', 'copy-provider-search-js', 'copy-registration-upload-js', 'copy-user-guide-js', 'copy-assets', 'merge-css',
    (done) => {
        done();
    }));