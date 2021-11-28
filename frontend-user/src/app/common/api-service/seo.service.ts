
export class MetaTag {
    name: string;
    value: string;
    isFacebook: boolean;

    constructor(name: string, value: string, isFacebook: boolean) {
        this.name = name;
        this.value = value;
        this.isFacebook = isFacebook;
    }
}

import { Injectable } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';
import { ApiService } from './api.service';

@Injectable({
    providedIn: 'root'
})
export class SEOService {

    private urlMeta: string = "og:url";
    private titleMeta: string = "og:title";
    private descriptionMeta: string = "og:description";
    private imageMeta: string = "og:image";
    private secureImageMeta: string = "og:image:secure_url";
    private twitterTitleMeta: string = "twitter:text:title";
    private twitterImageMeta: string = "twitter:image";

    constructor(private titleService: Title, private metaService: Meta, private api: ApiService) { }

    public setTitle(title: string): void {
        this.titleService.setTitle(title);
    }

    // public setSocialMediaTags(url: string, title: string, description: string, image: string): void {
    //     var imageUrl = `http://www.hce.edu.vn${image}`;
    //     var tags = [
    //         new MetaTag(this.urlMeta, url),
    //         new MetaTag(this.titleMeta, title),
    //         new MetaTag(this.descriptionMeta, description),
    //         new MetaTag(this.imageMeta, imageUrl),
    //         new MetaTag(this.secureImageMeta, imageUrl),
    //         new MetaTag(this.twitterTitleMeta, title, false),
    //         new MetaTag(this.twitterImageMeta, imageUrl, false)
    //     ];
    //     this.setTags(tags);
    // }

    public setSocialMediaTagsGoogle(description: string, keywords: string): void {
      
        let text = this.api.stripHtml(description);
        var tags = [
            new MetaTag('description', text.slice(0, 200) + '...', false),
            new MetaTag('keywords', keywords, false)
        ];
        this.setTags(tags);
    }

    public setSocialMediaTagsFacebook(title: string, image: string, url: string, description: string): void {

        let text = this.api.stripHtml(description);

        // var imageUrl = `http://www.hce.edu.vn${image}`;
        var tags = [
            new MetaTag('og:title', title, true),
            new MetaTag('og:image', image, true),
            new MetaTag('og:url', url, true),
            new MetaTag('og:description', text.slice(0, 200) + '...', true)
        ];
        this.setTags(tags);
    }


    public setSocialMediaTagsTwitter(title: string, image: string, description: string,): void {
        // var imageUrl = `http://www.hce.edu.vn${image}`;
        let text = this.api.stripHtml(description);
        var tags = [
            new MetaTag('twitter:title', title, false),
            new MetaTag('twitter:image', image, false),
            new MetaTag('twitter:description', text.slice(0, 200) + '...', false),
        ];
        this.setTags(tags);
    }


    private setTags(tags: MetaTag[]): void {
        // console.log(tags);
        tags.forEach(siteTag => {
            if (siteTag.isFacebook) {
                this.metaService.getTag(`property='${siteTag.name}'`);
                this.metaService.updateTag({ property: siteTag.name, content: siteTag.value });
            } else {
                this.metaService.getTag(`name='${siteTag.name}'`);
                this.metaService.updateTag({ name: siteTag.name, content: siteTag.value });
            }

        });
    }


}

